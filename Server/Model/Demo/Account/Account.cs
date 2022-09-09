namespace ET
{
    public enum AccountType
    {
        General = 0,
        BlackList = 1,
    }
    
    public class Account: Entity, IAwake
    {
        public string AccountName; //账号名
        public string Password; //账户密码
        public long CreateTime; //账号创建时间
        public int AccountType; //账号类型
        
        
        //不可修改变量
        //--------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 账户ID
        /// 唯一值，到时动态生成一个
        /// </summary>
        public long AccountID;

        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountType AccountTypeMy;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age;

        /// <summary>
        /// 用户使用沙盘次数
        /// </summary>
        public int UseOfNumber;

        /// <summary>
        /// 距用户上一次玩一盘沙盘游戏的时间
        /// </summary>
        public string IntervalTime;

        //--------------------------------------------------------------------------------------------------------
        //可以修改变量

        /// <summary>
        /// 班级年级
        /// </summary>
        public string ClassGrade;

        /// <summary>
        /// 学号
        /// </summary>
        public int StudentID;

        //--------------------------------------------------------------------------------------------------------
    }
}