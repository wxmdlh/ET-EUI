using System.Collections.Generic;

namespace ET
{
    public class MongoDBAccount: IAccount
    {
        public async ETTask Insert(Account account, Session session)
        {
            Log.Debug("在MongoDB中Account表增加一条记录");
            Game.EventSystem.Publish(new MongoDBAccountInsert() { AccountMy = account, SessionMy = session });
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(Account account, Session session, string field, string updateContext)
        {
            Log.Debug("在MongoDB中Account表更新一条Field的内容");
            Game.EventSystem.Publish(new MongoDBAccountUpdateFieldContext()
            {
                AccountMy = account, SessionMy = session, Field = field, UpdateContext = updateContext
            });
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(Account account, Session session)
        {
            Log.Debug("在MongoDB中Account表中删除一条记录");
            Game.EventSystem.Publish(new MongoDBAccountRemove() { AccountMy = account, SessionMy = session, });
            await ETTask.CompletedTask;
        }
    }

    public struct MongoDBAccountInsert
    {
        public Account AccountMy;
        public Session SessionMy;
    }

    public struct MongoDBAccountUpdateFieldContext
    {
        public Account AccountMy;
        public Session SessionMy;
        public string Field;
        public string UpdateContext;
    }

    public struct MongoDBAccountRemove
    {
        public Account AccountMy;
        public Session SessionMy;
    }
}