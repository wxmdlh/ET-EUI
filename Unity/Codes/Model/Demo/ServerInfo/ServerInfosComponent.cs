using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 在客户端SceneFactory上添加
    /// 记录游戏客户端下发的区服信息
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfosComponent:Entity,IAwake,IDestroy
    {
        public List<ServerInfo> ServerInfoList = new List<ServerInfo>();
    }
}