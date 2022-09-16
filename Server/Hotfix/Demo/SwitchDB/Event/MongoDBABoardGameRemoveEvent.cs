namespace ET
{
    public class MongoDBABoardGameRemoveEvent: AEvent<MongoDBABoardGameRemove>
    {
        protected override void Run(MongoDBABoardGameRemove a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBABoardGameRemove a)
        {
            Log.Debug("在MongoDB中ABoardGame表中，一条记录已删除");
            //数据删除
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone()).Remove<ABoardGame>(a.ABoardGameMy.Id);
        }
    }
}