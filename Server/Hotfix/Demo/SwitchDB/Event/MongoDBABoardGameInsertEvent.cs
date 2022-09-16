namespace ET
{
    public class MongoDBABoardGameInsertEvent: AEvent<MongoDBABoardGameInsert>
    {
        protected override void Run(MongoDBABoardGameInsert a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBABoardGameInsert a)
        {
            Log.Debug("在MongoDB中ABoardGame表中，一条记录已插入");
            //数据保存
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone()).Save<ABoardGame>(a.ABoardGameMy);
        }
    }
}