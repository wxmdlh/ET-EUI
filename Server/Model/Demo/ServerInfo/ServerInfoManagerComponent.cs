using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 挂在服务器端SceneFactory上
    /// 服务器上保存 服务器列表信息的类
    /// </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfoManagerComponent:Entity,IAwake,IDestroy,ILoad
    {
        public List<ServerInfo> ServerInfos = new List<ServerInfo>();
    }
}