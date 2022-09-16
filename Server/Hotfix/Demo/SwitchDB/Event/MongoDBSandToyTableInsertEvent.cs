namespace ET
{
    public class MongoDBSandToyTableInsertEvent: AEvent<MongoDBSandToyTableInsert>
    {
        protected override void Run(MongoDBSandToyTableInsert a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBSandToyTableInsert a)
        {
            Log.Debug("在MongoDB中SandToyTable表中，一条记录已插入");
            //数据保存
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone()).Save<SandToyTable>(a.SandToyTableMy);
        }
    }
}