using System;
using NLog.LayoutRenderers.Wrappers;

namespace ET
{
    /// <summary>
    /// Account服务器向LoginCenter服务器发送消息处理函数
    /// </summary>
    [ActorMessageHandler]
    public class A2L_LoginAccountRequestHandler: AMActorRpcHandler<Scene, A2L_LoginAccountRequest, L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    reply();
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                //通过区服信息，拿到Gate网关地址
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, accountId);
                //登录中心服通知Gate网关服，踢玩家下线 的消息
                //也可以使用 ActorMessageSenderComponent.Instance.Call来发送消息
                //MessageHelper.CallActor实际也是调用   ActorMessageSenderComponent.Instance.Call
                var g2LDisconnectGateUnit =
                        (G2L_DisconnectGateUnit) await MessageHelper.CallActor(gateConfig.InstanceId,
                            new L2G_DisconnectGateUnit() { AccountId = accountId });
                response.Error = g2LDisconnectGateUnit.Error;
                reply();
            }
        }
    }
}