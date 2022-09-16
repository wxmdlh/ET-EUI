namespace ET
{
    public interface ISandToyTable
    {
        ETTask Insert(SandToyTable sandToyTable, Session session);
        ETTask UpdateFieldContext(SandToyTable sandToyTable, Session session, string field, string updateContext);
        ETTask Remove(SandToyTable sandToyTable, Session session);
    }
}