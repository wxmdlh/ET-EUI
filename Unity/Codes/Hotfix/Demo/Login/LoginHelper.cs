using System;

namespace ET
{
    [FriendClass(typeof (AccountInfoComponent))]
    public static class LoginHelper
    {
        // public static async ETTask Login(Scene zoneScene, string address, string account, string password)
        // {
        //     try
        //     {
        //         // 创建一个ETModel层的Session
        //         R2C_Login r2CLogin;
        //         Session session = null;
        //         try
        //         {
        //             session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
        //             {
        //                 r2CLogin = (R2C_Login) await session.Call(new C2R_Login() { Account = account, Password = password });
        //             }
        //         }
        //         finally
        //         {
        //             session?.Dispose();
        //         }
        //
        //         // 创建一个gate Session,并且保存到SessionComponent中
        //         Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
        //         gateSession.AddComponent<PingComponent>();
        //         zoneScene.AddComponent<SessionComponent>().Session = gateSession;
        //
        //         G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
        //             new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId});
        //
        //         Log.Debug("登陆gate成功!");
        //
        //         Game.EventSystem.PublishAsync(new EventType.LoginFinish() {ZoneScene = zoneScene}).Coroutine();
        //     }
        //     catch (Exception e)
        //     {
        //         Log.Error(e);
        //     }
        // }

        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2CLoginAccount = null;
            Session accountSession = null;

            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);
                a2CLoginAccount = (A2C_LoginAccount) await accountSession.Call(new C2A_LoginAccount() { AccountName = account, Password = password });
            }
            catch (Exception e)
            {
                accountSession?.Dispose();
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CLoginAccount.Error != ErrorCode.ERR_Success)
            {
                accountSession?.Dispose();
                return a2CLoginAccount.Error;
            }

            //将该Session赋值给当前场景上的Session
            zoneScene.AddComponent<SessionComponent>().Session = accountSession;
            //心跳检测机制 用于服务器端检测客户端是否在线。服务器端每两秒给客户端发一次消息。
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            //将当前Token和AccountId进行保存
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2CLoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2CLoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="zoneScene"></param>
        /// <returns></returns>
        public static async ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2CGetServerInfos = null;

            try
            {
                a2CGetServerInfos = (A2C_GetServerInfos) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CGetServerInfos.Error != ErrorCode.ERR_Success)
            {
                return a2CGetServerInfos.Error;
            }

            //对游戏服务器下发的信息进行保存
            foreach (var serverInfoProto in a2CGetServerInfos.ServerInfosList)
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                //保存服务器中相关信息
                serverInfo.FromMessage(serverInfoProto);
                zoneScene.GetComponent<ServerInfosComponent>().Add(serverInfo);
            }

            return ErrorCode.ERR_Success;
        }

        /// <summary>
        /// 游戏客户端请求创建角色
        /// </summary>
        /// <param name="zoneScene"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async ETTask<int> CreateRole(Scene zoneScene, string name)
        {
            A2C_CreateRole a2CCreateRole = null;

            try
            {
                a2CCreateRole = (A2C_CreateRole) await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRole()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    Name = name,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CCreateRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CCreateRole.Error.ToString());
                return a2CCreateRole.Error;
            }

            //创建角色操作

            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
            //通过FromMessage方法将 服务器端 Proto字段，转化为RoleInfo类中字段
            newRoleInfo.FromMessage(a2CCreateRole.RoleInfo);
            zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Add(newRoleInfo);

            return ErrorCode.ERR_Success;
        }
    }
}