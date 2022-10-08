using System;
using System.Text.RegularExpressions;

namespace ET
{
    /// <summary>
    /// 用户登录账号验证逻辑
    /// </summary>
    [FriendClass(typeof (Account))]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        //当调用Action 委托的时候，实际是游戏服务器端向游戏客户端回复一条消息。
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            //判断当前请求到的是否是Account服务器
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前SceneType为{session.DomainScene().SceneType}");
                //因为没有向客户端回复消息，所以，不需要等待在断开
                session.Dispose();
            }

            //通过验证后，需及时将该组件移除，否则5秒后自动断开连接
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //处理一个玩家的多次登录请求
            //处理同一个玩家同一个连接同一种消息的多次请求
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            #region 判断请求的账号和密码

            //当账户名或者密码为空时
            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNull;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //限定账户必须为6-15位的数字、字母
            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //限定密码必须为6-15位的数字、字母
            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_PasswordFormError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            #endregion

            //当在某个代码段中使用了类的实例，而希望无论因为什么原因，只要离开了这个代码段就自动调用这个类实例的Dispose。 要达到这样的目的，用try...catch来捕捉异常也是可以的，但用using也很方便。
            using (session.AddComponent<SessionLockingComponent>())
            {
                //两个人从不同地方创建同一个账号时，会有两个session，运行两次Run()，就到导致一前一后创建两个同名的账号，从而导致查询时的错乱
                //用于锁定，只有一个账号操作完后，另一个账号才能进来操作。给异步逻辑加个锁。
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    //从数据库中查询账号是否存在，如果存在在进一步判断密码是否正确，如果不存在则创建该账号并保存

                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));
                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0) //账号存在
                    {
                        //拿到第一个账号
                        account = accountInfoList[0];
                        session.AddChild(account);
                        //如果账号类型为被禁止,就返回
                        if (account.AccountType == (int) AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        //如果密码不匹配则返回
                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else //账号不存在，自动注册
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password = request.Password;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = (int) AccountType.General;
                        //使用GetZoneDB来获取游戏区服。 session.DomainZone()是代表1 2 3 区服的概念。   
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                    }

                    //实现从客户端向登录账号中心服务器发送消息。
                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long loginCenterInstanceId = startSceneConfig.InstanceId;
                    //Actor 消息发送组件ActorMessageSenderComponent
                    var loginAccountResponse =
                            (L2A_LoginAccountResponse) await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId,
                                new A2L_LoginAccountRequest() { AccountId = account.InstanceId });
                    if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = loginAccountResponse.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose();
                        return;
                    }

                    //实现顶号操作,同一个账号，前一个账号会被后一个账号登录后顶下来
                    long accoutnSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    Session otherSession = Game.EventSystem.Get(accoutnSessionInstanceId) as Session;
                    //只发送消息 单条消息使用示例
                    otherSession?.Send(new A2C_Disconnect() { Error = 0 });
                    otherSession?.Disconnect().Coroutine();
                    //更新玩家，将先前玩家踢下线，更新后面玩家
                    session.DomainScene().Domain.GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);

                    //十分钟后还没操作/还没进入游戏，将玩家踢下线
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);

                    //生成每一个账号专属的Token令牌
                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);
                    //将存储在TokenComponent里的当前账号Id删除，为当前账号重新添加Token令牌
                    //session通过DomainScene得到Account Scene ，从而得到TokenComponent
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);

                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}