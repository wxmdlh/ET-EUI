using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>
    /// 显示当前服务器 区服列表
    /// </summary>
    [FriendClass(typeof (DlgServer))]
    [FriendClass(typeof (ServerInfosComponent))]
    public static class DlgServerSystem
    {
        public static void RegisterUIEvent(this DlgServer self)
        {
            self.View.E_EnterServerButton.AddListenerAsync(() => { return self.OnConfirmClickHandler(); });
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnScrollItemRefresHandler(transform, index);
            });
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {
            int count = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList.Count;
        }

        //============================================================================================================================================

        public static async ETTask OnConfirmClickHandler(this DlgServer self)
        {
        }

        public static void OnScrollItemRefresHandler(this DlgServer self, Transform transform, int index)
        {
        }
    }
}