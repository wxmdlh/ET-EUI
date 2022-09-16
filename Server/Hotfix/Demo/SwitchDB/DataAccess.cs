using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace ET
{
    [FriendClass(typeof (Account))]
    [FriendClass(typeof (ABoardGame))]
    [FriendClass(typeof (SandToyTable))]
    public static class DataAccess
    {
        #region Switch

        //private static readonly string db = "SQLite";
        // public static readonly string db = "MongoDB";

        /// <summary>
        /// 插入一条文档 到Account表中
        /// </summary>
        /// <returns></returns>
        public static IAccount InsertAccount()
        {
            IAccount result = null;

            switch (DBConstName.DBName)
            {
                case "MongoDB":
                    result = new MongoDBAccount();
                    break;
                case "SQLite":
                    result = new SQLiteAccount();
                    break;
            }

            return result;
        }

        /// <summary>
        /// 插入一条文档 到ABoardGame表中
        /// </summary>
        /// <returns></returns>
        public static IABoardGame InsertABoardGame()
        {
            IABoardGame result = null;

            switch (DBConstName.DBName)
            {
                case "MongoDB":
                    result = new MongoDBABoardGame();
                    break;
                case "SQLite":
                    result = new SQLiteABoardGame();
                    break;
            }

            return result;
        }

        /// <summary>
        /// 插入一条文档 到SandToyTable表中
        /// </summary>
        /// <returns></returns>
        public static ISandToyTable InsertSandToyTable()
        {
            ISandToyTable result = null;

            switch (DBConstName.DBName)
            {
                case "MongoDB":
                    result = new MongoDBSandToyTable();
                    break;
                case "SQLite":
                    result = new SQLiteSandToyTable();
                    break;
            }

            return result;
        }

        /// <summary>
        /// 查询Account表中，与输入 AccountName相同的数据项
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="accountName">传入账户名称</param>
        /// <returns>返回相同AccountName的Account列表</returns>
        public static async ETTask<List<Account>> QueryAccounts(Session session, string accountName)
        {
            List<Account> accounts = new List<Account>();

            switch (DBConstName.DBName)
            {
                case "MongoDB":
                    accounts = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(accountName));
                    break;
                case "SQLite":

                    break;
            }

            return accounts;
        }

        /// <summary>
        /// 查询ABoardGame表中，与输入 AccountID相同的数据项
        /// </summary>
        /// <param name="session"></param>
        /// <param name="accountID">Account的AccountID</param>
        /// <returns>返回相同AccountID的ABoardGame列表</returns>
        public static async ETTask<List<ABoardGame>> QueryABoardGame(Session session, long accountID)
        {
            List<ABoardGame> aBoardGames = new List<ABoardGame>();

            switch (DBConstName.DBName)
            {
                case "MongoDB":
                    aBoardGames = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<ABoardGame>(d => d.OwnerAsID == accountID);
                    break;
                case "SQLite":

                    break;
            }

            return aBoardGames;
        }

        /// <summary>
        /// 查询SandToyTable表中，与输入 GameID相同的数据项
        /// </summary>
        /// <param name="session"></param>
        /// <param name="gameID">ABoardGame的GameID</param>
        /// <returns>返回相同GameID的SandToyTable列表</returns>
        public static async ETTask<List<SandToyTable>> QuerySandToyTable(Session session, long gameID)
        {
            List<SandToyTable> sandToyTables = new List<SandToyTable>();

            switch (DBConstName.DBName)
            {
                case "MongoDB":

                    sandToyTables = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<SandToyTable>(d => d.BelongABoardGameID == gameID);
                    break;
                case "SQLite":

                    break;
            }

            return sandToyTables;
        }

        #endregion

        #region 反射

        // //private static readonly string AssemblyName = "利用抽象工厂实现_原型";
        // private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        // private static readonly string db = "MongoDB";
        //
        //
        // public static IAccount CreateAccount()
        // {
        //     string className = Assembly.GetExecutingAssembly().GetName().Name + "." + db + "Account";
        //     Log.Debug($"className {className}");   
        //     return (IAccount)Assembly.Load(AssemblyName).CreateInstance(className);
        // }

        #endregion
    }
}