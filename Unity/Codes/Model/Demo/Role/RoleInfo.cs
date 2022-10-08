namespace ET
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze,
    }
    /// <summary>
    /// 为了实现在客户端及服务器端  关于Role信息的存取，因此需要在服务器端引用客户端
    /// </summary>
    public class RoleInfo:Entity,IAwake
    {
        public string Name;
        public int ServerId;
        public int State;
        public long AccountId;
        public long LastLoginTime;
        public long CreateTime;
    }
}