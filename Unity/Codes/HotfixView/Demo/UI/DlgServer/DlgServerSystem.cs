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
    [FriendClass(typeof (ServerInfo))]
    public static class DlgServerSystem
    {
        public static void RegisterUIEvent(this DlgServer self)
        {
            self.View.E_ConfirmButton.AddListenerAsync(() => { return self.OnConfirmClickHandler(); });
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnScrollItemRefresHandler(transform, index);
            });
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {
            int count = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList.Count;
            self.AddUIScrollItems(ref self.ScrollItemServerTests, count);
            self.View.E_ServerListLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void HideWindow(this DlgServer self)
        {
            self.RemoveUIScrollItems(ref self.ScrollItemServerTests);
        }

        //============================================================================================================================================

        /// <summary>
        /// 列表响应事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <param name="index"></param>
        public static void OnScrollItemRefresHandler(this DlgServer self, Transform transform, int index)
        {
            Scroll_Item_serverTest serverTest = self.ScrollItemServerTests[index].BindTrans(transform);
            ServerInfo info = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList[index];
            //设置背景颜色
            serverTest.EI_serverTestImage.color = info.Id == self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId?
                    Color.red : Color.cyan;
            //设置选项文字
            serverTest.E_serverTestTipText.SetText(info.ServerName);
            serverTest.E_SelectButton.AddListener(() => { self.OnSelectServerItemHandler(info.Id); });
        }

        /// <summary>
        /// 滚动列表选项 按钮响应事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="infoId"></param>
        public static void OnSelectServerItemHandler(this DlgServer self, long serverId)
        {
            self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = int.Parse(serverId.ToString());
            Log.Debug($"当前选择的服务器Id是:{serverId}");
            self.View.E_ServerListLoopVerticalScrollRect.RefillCells();
        }

        /// <summary>
        /// 按钮响应事件
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask OnConfirmClickHandler(this DlgServer self)
        {
            bool isSelect = self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId != 0;

            if (!isSelect)
            {
                Log.Error("请先选择区服");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.GetRoles(self.ZoneScene());

                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                //关闭服务器列表选择页面，打开创建角色页面
                self.ZoneScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Server);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}