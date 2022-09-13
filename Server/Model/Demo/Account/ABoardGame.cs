
namespace ET
{
    public class ABoardGame: Entity, IAwake
    {
        //不可修改变量
        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 一局游戏档案ID
        /// 唯一值  到时动态生成
        /// </summary>
        public long GameID;

        /// <summary>
        /// 此局游戏制作日期
        /// </summary>
        public string MakeTime;

        /// <summary>
        /// 此局游戏场景类型
        /// </summary>
        public string SceneType;

        /// <summary>
        /// 此局游戏用时
        /// </summary>
        public string UsageTime;

        /// <summary>
        /// 沙盘图片路径
        /// </summary>
        public string ImagePath;

        /// <summary>
        /// 该局游戏所属用户ID
        /// </summary>
        public long OwnerAsID;

        //----------------------------------------------------------------------------------------------------
        //可以修改变量

        /// <summary>
        /// 沙盘名称
        /// </summary>
        public string SandTableName;

        /// <summary>
        /// 来访者档案
        /// </summary>
        public string VisitorFile;

        /// <summary>
        /// 来访者结束后说
        /// </summary>
        public string VisitorSaid;

        /// <summary>
        /// 沙盘分析
        /// </summary>
        public string SandTableAnalysis;

        //----------------------------------------------------------------------------------------------------
    }
}