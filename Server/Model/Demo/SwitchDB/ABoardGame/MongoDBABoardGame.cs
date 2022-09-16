using System.Collections.Generic;

namespace ET
{
    public class MongoDBABoardGame: IABoardGame
    {
        public async ETTask Insert(ABoardGame aBoardGame, Session session)
        {
            Log.Debug("在MongoDB中ABoardGame表增加一条记录");
            Game.EventSystem.Publish(new MongoDBABoardGameInsert() { ABoardGameMy = aBoardGame, SessionMy = session });
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(ABoardGame aBoardGame, Session session, string field, string updateContext)
        {
            Log.Debug("在MongoDB中ABoardGame表更新一条Field的内容");
            Game.EventSystem.Publish(new MongoDBABoardGameUpdateFieldContext()
            {
                ABoardGameMy = aBoardGame, SessionMy = session, Field = field, UpdateContext = updateContext
            });
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(ABoardGame aBoardGame, Session session)
        {
            Log.Debug("在MongoDB中ABoardGame表中删除一条记录");
            Game.EventSystem.Publish(new MongoDBABoardGameRemove() { SessionMy = session, ABoardGameMy = aBoardGame });
            await ETTask.CompletedTask;
        }
    }

    public struct MongoDBABoardGameInsert
    {
        public ABoardGame ABoardGameMy;
        public Session SessionMy;
    }

    public struct MongoDBABoardGameUpdateFieldContext
    {
        public ABoardGame ABoardGameMy;
        public Session SessionMy;
        public string Field;
        public string UpdateContext;
    }

    public struct MongoDBABoardGameRemove
    {
        public ABoardGame ABoardGameMy;
        public Session SessionMy;
    }
}