using System;

namespace ET
{
    /// <summary>
    /// 登录中心服 向 Gate网关服 发送消息 事件定义
    /// </summary>
    public class L2G_DisconnectUnitHandler: AMActorRpcHandler<Scene, L2G_DisconnectGateUnit, G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                //得到该账号操控的角色
                Player gateUnit = playerComponent.Get(accountId);

                if (gateUnit == null)
                {
                    reply();
                    return;
                }

                playerComponent.Remove(accountId);
                gateUnit.Dispose();
            }

            reply();
        }
    }
}