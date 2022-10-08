﻿using System;

namespace ET
{
    public class ServerInfoManagerComponentSystemAwakeSystem: AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public class ServerInfoManagerComponentSystemDestroySystem: DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var serverInfo in self.ServerInfos)
            {
                serverInfo?.Dispose();
            }

            self.ServerInfos.Clear();
        }
    }

    public class ServerInfoManagerComponentSystemLoadSystem: LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    [FriendClass(typeof(ServerInfoManagerComponent))]
    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            var serverInfoList = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if (serverInfoList == null || serverInfoList.Count <= 0)
            {
                Log.Error("ServerInfo count is zero");
                
                self.ServerInfos.Clear();

                //得到所有服务器
                var serverInfoConfigs = ServerInfoConfigCategory.Instance.GetAll();

                foreach (var info in serverInfoConfigs.Values)
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(info.Id);
                    newServerInfo.ServerName = info.ServerName;
                    newServerInfo.Status = (int) ServerStatus.Normal;
                    self.ServerInfos.Add(newServerInfo);
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(newServerInfo);
                }
            }

            self.ServerInfos.Clear();

            foreach (var serverInfo in serverInfoList)
            {
                self.AddChild(serverInfo);
                self.ServerInfos.Add(serverInfo);
            } 
        }
    }
}