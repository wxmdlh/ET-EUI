namespace ET
{
    public class MongoDBSandToyTableRemoveEvent: AEvent<MongoDBSandToyTableRemove>
    {
        protected override void Run(MongoDBSandToyTableRemove a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBSandToyTableRemove a)
        {
            Log.Debug("在MongoDB中SandToyTable表中，一条记录已删除");
            //数据删除
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone())
                    .Remove<SandToyTable>(a.SandToyTableMy.Id);
        }
    }
}