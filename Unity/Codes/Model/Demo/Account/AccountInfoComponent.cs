using System;

namespace ET
{
    /// <summary>
    /// 保存令牌 登录账号Id
    /// 在SceneFactory上添加
    /// 登录成功时保存账号，上述信息
    /// </summary>
    [ChildType]
    [ComponentOf(typeof (Scene))]
    public class AccountInfoComponent: Entity, IAwake, IDestroy
    {
        public string Token = string.Empty;
        public long AccountId;
    }
}