namespace ET
{
    public class SQLiteSandToyTable: ISandToyTable
    {
        public async ETTask Insert(SandToyTable sandToyTable, Session session)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(SandToyTable sandToyTable, Session session, string field, string updateContext)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(SandToyTable sandToyTable,Session session)
        {
            await ETTask.CompletedTask;
        }
    }
}