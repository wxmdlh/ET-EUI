using System.Collections.Generic;

namespace ET
{
    public class SQLiteAccount: IAccount
    {
        public async ETTask Insert(Account account, Session session)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask UpdateFieldContext(Account account, Session session, string field, string updateContext)
        {
            await ETTask.CompletedTask;
        }

        public async ETTask Remove(Account account,Session session)
        {
            await ETTask.CompletedTask;
        }
    }
}