using System.Collections.Generic;

namespace ET
{
    public interface IAccount
    {
        ETTask Insert(Account account, Session session);
        ETTask UpdateFieldContext(Account account, Session session, string field, string updateContext);
        ETTask Remove(Account account, Session session);
    }
}