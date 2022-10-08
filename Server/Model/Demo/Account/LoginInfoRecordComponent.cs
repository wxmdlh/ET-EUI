using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 登录中心服
    /// 游戏服务器之间通信的消息
    /// 使客户端与Gate网关服务器断开连接  就需要登录中心服务器
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class LoginInfoRecordComponent:Entity,IAwake,IDestroy
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}