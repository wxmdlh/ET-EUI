using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 登录中心服
    /// 游戏服务器之间通信的消息
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class LoginInfoRecordComponent:Entity,IAwake,IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}