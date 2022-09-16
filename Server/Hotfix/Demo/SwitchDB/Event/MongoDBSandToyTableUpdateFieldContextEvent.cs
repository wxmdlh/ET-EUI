namespace ET
{
    public class MongoDBSandToyTableUpdateFieldContextEvent:AEvent<MongoDBSandToyTableUpdateFieldContext>
    {
        protected override void Run(MongoDBSandToyTableUpdateFieldContext a)
        {
            RealRun(a).Coroutine();
        }
        
        private async ETTask RealRun(MongoDBSandToyTableUpdateFieldContext a)
        {
            Log.Debug("在MongoDB中SandToyTable表中，一条记录已更新");
            //数据更新
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone())
                    .FindOneAndUpdateAsync<SandToyTable>(a.SandToyTableMy, a.Field, a.UpdateContext);
        }
    }
}