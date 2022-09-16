namespace ET
{
    public class MongoDBAccountUpdateFieldContextEvent: AEvent<MongoDBAccountUpdateFieldContext>
    {
        protected override void Run(MongoDBAccountUpdateFieldContext a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBAccountUpdateFieldContext a)
        {
            Log.Debug("在MongoDB中Account表中，一条记录已更新");
            //数据更新
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone())
                    .FindOneAndUpdateAsync<Account>(a.AccountMy, a.Field, a.UpdateContext);
        }
    }
}