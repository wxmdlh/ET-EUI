namespace ET
{
    public static class DisconnectHelper
    {
        /// <summary>
        /// 为了解决直接调用session.Dispose时，reply()消息可能还没发送完，因为是网络消息，传送需要时间
        /// 可能会导致客户端无法接收到服务器端的消息
        /// 所以需要延时在断开session
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);

            //为了防止1秒后，session改变，所以使用1秒前后的InstanceId进行对比，实现session的安全释放
            if (self.InstanceId != instanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}