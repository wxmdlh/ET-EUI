namespace ET
{
    public class AccountSessionComponentDestroySystem: DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }

    [FriendClass(typeof (AccountSessionsComponent))]
    public static class AccountSessionsComponentSystem
    {
        /// <summary>
        /// 通过accountId得到instanceId
        /// </summary>
        /// <param name="self"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static long Get(this AccountSessionsComponent self, long accountId)
        {
            if (!self.AccountSessionDictionary.TryGetValue(accountId, out long instanceId))
            {
                return 0;
            }

            return instanceId;
        }

        /// <summary>
        /// 将accountId和instanceId添加进字典
        /// </summary>
        /// <param name="self"></param>
        /// <param name="accountId"></param>
        /// <param name="instanceId"></param>
        public static void Add(this AccountSessionsComponent self, long accountId, long instanceId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary[accountId] = instanceId;
                return;
            }

            self.AccountSessionDictionary.Add(accountId, instanceId);
        }

        /// <summary>
        /// 从字典中移除一个数据项
        /// </summary>
        /// <param name="self"></param>
        /// <param name="accountId"></param>
        public static void Remove(this AccountSessionsComponent self, long accountId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary.Remove(accountId);
            }
        }
    }
}