using System;

namespace ET
{
    [ObjectSystem]
    public class AccountInfoComponentDestroySystem: DestroySystem<AccountInfoComponent>
    {
        public override void Destroy(AccountInfoComponent self)
        {
            self.Token = String.Empty;
            self.AccountId = 0;
        }
    }

    public static class AccountInfoComponentSystem
    {
    }
}