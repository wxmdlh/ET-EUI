namespace ET
{
    public enum ServerStatus
    {
        Normal = 0,
        Stop = 1,
    }

    /// <summary>
    /// 获取服务器列表类
    /// </summary>
    [ChildType]
    public class ServerInfo: Entity, IAwake
    {
        public int Status;
        public string ServerName;
    }
}