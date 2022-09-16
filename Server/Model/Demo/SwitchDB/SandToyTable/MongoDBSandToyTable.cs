namespace ET
{
    public class MongoDBSandToyTable: ISandToyTable
    {
        public async ETTask Insert(SandToyTable sandToyTable, Session session)
        {
            Log.Debug("在MongoDB中SandToyTable表增加一条记录");
            Game.EventSystem.Publish(new MongoDBSandToyTableInsert() { SandToyTableMy = sandToyTable, SessionMy = session });
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(SandToyTable sandToyTable, Session session, string field, string updateContext)
        {
            Log.Debug("在MongoDB中SandToyTable表更新一条Field的内容");
            Game.EventSystem.Publish(new MongoDBSandToyTableUpdateFieldContext()
            {
                SandToyTableMy = sandToyTable, SessionMy = session, Field = field, UpdateContext = updateContext
            });
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(SandToyTable sandToyTable, Session session)
        {
            Log.Debug("在MongoDB中SandToyTable表中删除一条记录");
            Game.EventSystem.Publish(new MongoDBSandToyTableRemove() { SandToyTableMy = sandToyTable, SessionMy = session, });
            await ETTask.CompletedTask;
        }
    }

    public struct MongoDBSandToyTableInsert
    {
        public SandToyTable SandToyTableMy;
        public Session SessionMy;
    }

    public struct MongoDBSandToyTableUpdateFieldContext
    {
        public SandToyTable SandToyTableMy;
        public Session SessionMy;
        public string Field;
        public string UpdateContext;
    }

    public struct MongoDBSandToyTableRemove
    {
        public SandToyTable SandToyTableMy;
        public Session SessionMy;
    }
}