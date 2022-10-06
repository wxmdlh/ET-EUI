using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 用于存储每个账号的Token令牌信息
    /// 挂在服务器端SceneFactory上
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class TokenComponent: Entity, IAwake
    {
        public readonly Dictionary<long, string> TokenDic = new Dictionary<long, string>();
    }
}