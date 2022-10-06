using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 挂在服务器端SceneFactory上
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfoManagerComponent:Entity,IAwake,IDestroy,ILoad
    {
        public List<ServerInfo> ServerInfos = new List<ServerInfo>();
    }
}