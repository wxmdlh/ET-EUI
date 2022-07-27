namespace ET
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);

            //借助InstanceId释放组件，在ET内是常用的
            if (self.InstanceId != instanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}