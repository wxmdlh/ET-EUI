﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET
{
    /// <summary>
    /// 账号验证逻辑
    /// </summary>
    /// 
    [FriendClass(typeof (Account))]
    [FriendClass(typeof (ABoardGame))]
    [FriendClass(typeof (SandToyTable))]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前SceneType为{session.DomainScene().SceneType}");
                session.Dispose();
            }

            //通过验证后，需及时将该组件移除，否则5秒后自动断开连接
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //当在某个代码段中使用了类的实例，而希望无论因为什么原因，只要离开了这个代码段就自动调用这个类实例的Dispose。 要达到这样的目的，用try...catch来捕捉异常也是可以的，但用using也很方便。
            using (session.AddComponent<SessionLockingComponent>())
            {
                //用于锁定，只有一个账号操作完后，另一个账号才能进来操作。
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    //1.从数据库中查询账号是否存在，如果存在在进一步判断密码是否正确，如果不存在则创建该账号并保存
                    //数据查询
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));

                    Account account = null;
                    ABoardGame aBoardGame = null;
                    SandToyTable sandToyTable = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);

                        // // 如果账号存在，开始向ABoardGame中插入一个数据项
                        // //数据项插入
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .InsertBatch<ABoardGame>(new List<ABoardGame>()
                        //         {
                        //             new ABoardGame()
                        //             {
                        //                 GameID=123,
                        //             }
                        //         });
                        //
                        // //如果账号存在，开始向ABoardGame中插入一个数据项
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .InsertBatch<SandToyTable>(new List<SandToyTable>()
                        //         {
                        //             new SandToyTable()
                        //             {
                        //                 Order=1,
                        //             }
                        //         });

                        #region 如果该账号存在，ABoardGame表中GameID也存在，则更新MakeTime

                        // var aboardGameList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .Query<ABoardGame>(d => d.OwnerAsID == account.AccountID);
                        //
                        // if (aboardGameList != null && aboardGameList.Count > 0)
                        // {
                        //     aBoardGame = aboardGameList[0];
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<ABoardGame>(aBoardGame, "MakeTime", "2022/9/13 14:22:00 ");
                        // }

                        #endregion

                        #region 同一账号，多次插入不同ABoardGame

                        // aBoardGame = session.AddChild<ABoardGame>();
                        // aBoardGame.MakeTime = DateTime.Now.ToString();
                        // aBoardGame.OwnerAsID = account.AccountID;
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<ABoardGame>(aBoardGame);
                        //
                        // var aBoardGameList1 = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .Query<ABoardGame>(d => d.OwnerAsID.Equals(account.AccountID));
                        // if (aBoardGameList1 != null && aBoardGameList1.Count > 0)
                        // {
                        //     for (int i = 0; i < aBoardGameList1.Count; i++)
                        //     {
                        //         Log.Debug($"ABoardGame{i},制作时间为 {aBoardGameList1[i].MakeTime}");
                        //     }
                        //
                        //     DateTime.TryParse(aBoardGameList1[aBoardGameList1.Count-1].MakeTime, out var ultimate);
                        //     DateTime.TryParse(aBoardGameList1[aBoardGameList1.Count-2].MakeTime, out var penultimate);
                        //     System.TimeSpan t = ultimate - penultimate;
                        //
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<Account>(account, "IntervalTime", t.ToString());
                        //
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<Account>(account, "UseOfNumber", aBoardGameList1.Count.ToString());
                        // }

                        #endregion

                        #region 同一账号，同一ABoardGame下，沙具的插入

                        // sandToyTable = session.AddChild<SandToyTable>();
                        // sandToyTable.Number = 0;
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<SandToyTable>(sandToyTable);
                        //
                        // var aBoardGameList2 = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .Query<ABoardGame>(d => d.OwnerAsID.Equals(account.AccountID));
                        // if (aBoardGameList2 != null && aBoardGameList2.Count > 0)
                        // {
                        //     Log.Debug($"aBoardGameList2[0]存在");
                        //
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<SandToyTable>(sandToyTable, "BelongABoardGameID",
                        //                 aBoardGameList2[0].GameID.ToString());
                        // }

                        #endregion

                        #region 完整版一个账号如果存在 则多次插入不同的ABoardGame，每局ABoardGame，都可以插入多条SandToyTable

                        // aBoardGame = session.AddChild<ABoardGame>();
                        // aBoardGame.MakeTime = DateTime.Now.ToString();
                        // aBoardGame.OwnerAsID = account.AccountID;
                        // aBoardGame.GameID = RandomHelper.RandInt64();
                        // await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<ABoardGame>(aBoardGame);
                        //
                        // var aBoardGameList1 = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //         .Query<ABoardGame>(d => d.OwnerAsID.Equals(account.AccountID));
                        // if (aBoardGameList1 != null && aBoardGameList1.Count > 0)
                        // {
                        //     //更新Account表
                        //     DateTime.TryParse(aBoardGameList1[aBoardGameList1.Count - 1].MakeTime, out var ultimate);
                        //     DateTime.TryParse(aBoardGameList1[aBoardGameList1.Count - 2].MakeTime, out var penultimate);
                        //     System.TimeSpan t = ultimate - penultimate;
                        //     //更新Account表中IntervalTime
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<Account>(account, "IntervalTime", t.ToString());
                        //     //更新Account表中UseOfNumber
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        //             .FindOneAndUpdateAsync<Account>(account, "UseOfNumber", aBoardGameList1.Count.ToString());
                        //
                        //     //更新SandToyTable表
                        //     sandToyTable = session.AddChild<SandToyTable>();
                        //     sandToyTable.Number = 0;
                        //     sandToyTable.BelongABoardGameID = aBoardGameList1[aBoardGameList1.Count - 1].GameID;
                        //     await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<SandToyTable>(sandToyTable);
                        // }

                        #endregion

                        #region 删除表中一条数据项

                        var aBoardGameList2 = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                                .Query<ABoardGame>(d => d.OwnerAsID == account.AccountID);
                        if (aBoardGameList2 != null && aBoardGameList2.Count > 0)
                        {
                            Log.Debug($"删除了aBoardGameList2[aBoardGameList2.Count - 1] {aBoardGameList2[aBoardGameList2.Count - 1].GameID}");

                            await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                                    .Remove<ABoardGame>(aBoardGameList2[aBoardGameList2.Count - 1].Id);
                        }

                        #endregion

                        Log.Debug($"该账号存在{request.AccountName}");
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password = request.Password;
                        account.AccountType = (int) AccountType.General;
                        account.AccountID = RandomHelper.RandInt64();
                        //数据保存
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);

                        aBoardGame = session.AddChild<ABoardGame>();
                        //TODO...GameID的设定
                        aBoardGame.GameID = RandomHelper.RandInt64();
                        aBoardGame.MakeTime = DateTime.Now.ToString();
                        aBoardGame.OwnerAsID = account.AccountID;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<ABoardGame>(aBoardGame);

                        sandToyTable = session.AddChild<SandToyTable>();
                        sandToyTable.Number = 0;
                        sandToyTable.BelongABoardGameID = aBoardGame.GameID;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<SandToyTable>(sandToyTable);
                    }

                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue);

                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}