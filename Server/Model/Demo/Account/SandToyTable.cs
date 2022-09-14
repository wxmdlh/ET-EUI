namespace ET
{
    public class SandToyTable:Entity,IAwake
    {
        /// <summary>
        /// 沙具使用顺序
        /// </summary>
        public int Order;

        /// <summary>
        /// 沙具使用数量
        /// </summary>
        public int Number;

        /// <summary>
        /// 沙具象征意义
        /// </summary>
        public string SymbolicSignificance;

        /// <summary>
        /// 属于那一场游戏的GameID
        /// </summary>
        public long BelongABoardGameID;
    }
}