using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET
{
    /// <summary>
    /// 账号验证逻辑
    /// </summary>
    /// 
    [FriendClass(typeof (Account))]
    [FriendClass(typeof (ABoardGame))]
    [FriendClass(typeof (SandToyTable))]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前SceneType为{session.DomainScene().SceneType}");
                session.Dispose();
            }

            //通过验证后，需及时将该组件移除，否则5秒后自动断开连接
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //当在某个代码段中使用了类的实例，而希望无论因为什么原因，只要离开了这个代码段就自动调用这个类实例的Dispose。 要达到这样的目的，用try...catch来捕捉异常也是可以的，但用using也很方便。
            using (session.AddComponent<SessionLockingComponent>())
            {
                //用于锁定，只有一个账号操作完后，另一个账号才能进来操作。
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    //1.从数据库中查询账号是否存在，如果存在在进一步判断密码是否正确，如果不存在则创建该账号并保存
                    //数据查询
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));

                    Account account = null;
                    ABoardGame aBoardGame = null;
                    SandToyTable sandToyTable = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);

                        // // 如果账号存在，开始向ABoardGame中插入一个数据项
                        // //数据项插入
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .InsertBatch<ABoardGame>(new List<ABoardGame>()
                        //         {
                        //             new ABoardGame()
                        //             {
                        //                 GameID=123,
                        //             }
                        //         });
                        //
                        // //如果账号存在，开始向ABoardGame中插入一个数据项
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .InsertBatch<SandToyTable>(new List<SandToyTable>()
                        //         {
                        //             new SandToyTable()
                        //             {
                        //                 Order=1,
                        //             }
                        //         });

                        #region 如果该账号存在，ABoardGame表中GameID也存在，则更新MakeTime

                        // var aboardGameList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .Query<ABoardGame>(d => d.GameID.Equals("-2105361331"));
                        //
                        // if (aboardGameList != null && aboardGameList.Count > 0)
                        // {
                        //     aBoardGame = aboardGameList[0];
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<ABoardGame>(aBoardGame, "MakeTime", "2022/9/13 14:22:00 ");
                        // }

                        #endregion

                        Log.Debug($"该账号存在{request.AccountName}");
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password = request.Password;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = (int) AccountType.General;
                        //数据保存
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);

                        aBoardGame = session.AddChild<ABoardGame>();
                        aBoardGame.GameID = request.AccountName.Trim().GetHashCode();
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<ABoardGame>(aBoardGame);

                        sandToyTable = session.AddChild<SandToyTable>();
                        sandToyTable.Number = 0;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<SandToyTable>(sandToyTable);
                    }

                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);

                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}