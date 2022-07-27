using System.Linq;

namespace ET
{
    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        /// <summary>
        /// 添加Token
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="token"></param>
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.TokenDic.Add(key,token);
            self.TimeOutRemoveKey(key,token).Coroutine();
        }

        /// <summary>
        /// 通过Key获取Token
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(this TokenComponent self, long key)
        {
            string value = null;
            self.TokenDic.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 删除一个Token
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        public static void Remove(this TokenComponent self, long key)
        {
            if (self.TokenDic.ContainsKey(key))
            {
                self.TokenDic.Remove(key);
            }
        }

        /// <summary>
        /// 超时后，自动将Token删除
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="tokenKey"></param>
        public static async ETTask TimeOutRemoveKey(this TokenComponent self, long key, string tokenKey)
        {
            await TimerComponent.Instance.WaitAsync(36000000);
            string onlineToken = self.Get(key);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
            {
                self.Remove(key);
            }
        }
        
        
        
        
        
        
        
        
        
        
    }
}