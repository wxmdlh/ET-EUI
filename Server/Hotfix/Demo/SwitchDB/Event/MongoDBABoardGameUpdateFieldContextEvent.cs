namespace ET
{
    public class MongoDBABoardGameUpdateFieldContextEvent: AEvent<MongoDBABoardGameUpdateFieldContext>
    {
        protected override void Run(MongoDBABoardGameUpdateFieldContext a)
        {
            RealRun(a).Coroutine();
        }

        private async ETTask RealRun(MongoDBABoardGameUpdateFieldContext a)
        {
            Log.Debug("在MongoDB中Account表中，一条记录已更新");
            //数据更新
            await DBManagerComponent.Instance.GetZoneDB(a.SessionMy.DomainZone())
                    .FindOneAndUpdateAsync<ABoardGame>(a.ABoardGameMy, a.Field, a.UpdateContext);
        }
    }
}