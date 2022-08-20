using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent:Entity,IAwake,IDestroy
    {
        /// <summary>
        /// accountId instanceId
        /// </summary>
        public Dictionary<long, long> AccountSessionDictionary = new Dictionary<long, long>();
    }
}