namespace ET.SwitchDB
{
    public class MongoDBAccountInsertEvent: AEvent<MongoDBAccountInsert>
    {
        protected override void Run(MongoDBAccountInsert a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBAccountInsert a)
        {
            Log.Debug("在MongoDB中Account表中，一条记录已插入");
            //数据保存
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone()).Save<Account>(a.AccountMy);
        }
    }
}