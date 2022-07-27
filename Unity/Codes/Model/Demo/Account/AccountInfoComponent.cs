using System;

namespace ET
{
    /// <summary>
    /// 保存令牌等信息
    /// </summary>
    [ChildType]
    [ComponentOf(typeof (Scene))]
    public class AccountInfoComponent: Entity, IAwake, IDestroy
    {
        public string Token = string.Empty;
        public long AccountId;
    }
}