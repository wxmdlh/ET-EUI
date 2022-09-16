using System.Collections.Generic;

namespace ET
{
    public class SQLiteABoardGame: IABoardGame
    {
        public async ETTask Insert(ABoardGame aBoardGame, Session session)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(ABoardGame account, Session session, string field, string updateContext)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(ABoardGame aBoardGame, Session session)
        {
            await ETTask.CompletedTask;
        }
    }
}