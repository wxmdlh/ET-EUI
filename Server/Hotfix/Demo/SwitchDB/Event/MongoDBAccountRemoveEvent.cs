namespace ET
{
    public class MongoDBAccountRemoveEvent:AEvent<MongoDBAccountRemove>
    {
        protected override void Run(MongoDBAccountRemove a)
        {
            RealRun(a).Coroutine();
        }
        
        private async ETTask RealRun(MongoDBAccountRemove a)
        {
            Log.Debug("在MongoDB中Account表中，一条记录已删除");
            //数据删除
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone())
                    .Remove<ABoardGame>(a.AccountMy.Id);
        }
    }
}