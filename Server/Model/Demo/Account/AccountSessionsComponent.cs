using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 用于存储accountId   accoutnSessionInstanceId
    /// 挂在服务器端SceneFactory上
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent:Entity,IAwake,IDestroy
    {
        /// <summary>
        /// accountId  accoutnSessionInstanceId
        /// </summary>
        public Dictionary<long, long> AccountSessionDictionary = new Dictionary<long, long>();
    }
}