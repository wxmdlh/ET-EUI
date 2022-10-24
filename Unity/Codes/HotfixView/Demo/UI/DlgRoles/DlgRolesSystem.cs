using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof (DlgRoles))]
    [FriendClass(typeof (RoleInfosComponent))]
    [FriendClass(typeof (RoleInfo))]
    public static class DlgRolesSystem
    {
        public static void RegisterUIEvent(this DlgRoles self)
        {
            self.View.E_ConfirmButton.AddListenerAsync(() => { return self.OnConfirmClickHandler(); });
            self.View.E_CreateRoleButton.AddListenerAsync(() => { return self.OnCreateClickHandler(); });
            self.View.E_DeleteRoleButton.AddListenerAsync(() => { return self.OnDeleteClickHandler(); });
            self.View.E_RolesLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
            {
                self.OnScrollItemRefresHandler(transform, index);
            });
        }

        public static void ShowWindow(this DlgRoles self, Entity contextData = null)
        {
            self.RefreshRoleItems();
        }

        public static void RefreshRoleItems(this DlgRoles self)
        {
            int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
            self.AddUIScrollItems(ref self.ScrollItemRoles, count);
            self.View.E_RolesLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void OnScrollItemRefresHandler(this DlgRoles self, Transform transform, int index)
        {
            Scroll_Item_role scrollItemServerTest = self.ScrollItemRoles[index].BindTrans(transform);
            RoleInfo info = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos[index];
            scrollItemServerTest.E_RoleImage.color =
                    info.Id == self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId? Color.green : Color.gray;
            scrollItemServerTest.E_RoleNameText.SetText(info.Name);
            scrollItemServerTest.E_RoleButton.AddListener(() => { self.OnRoleItemClickHandler(info.Id); });
        }

        public static void OnRoleItemClickHandler(this DlgRoles self, long roleId)
        {
            self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = roleId;
            self.View.E_RolesLoopVerticalScrollRect.RefillCells();
        }

        public static async ETTask OnCreateClickHandler(this DlgRoles self)
        {
            string name = self.View.E_RoleNameInputField.text;

            if (string.IsNullOrEmpty(name))
            {
                Log.Warning("Name is null");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), name);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                self.RefreshRoleItems();
            }
            catch (Exception e)
            {
                Log.Error(string.Empty.ToString());
            }
        }

        public static async ETTask OnConfirmClickHandler(this DlgRoles self)
        {
            Log.Warning("开始游戏");
            await ETTask.CompletedTask;
        }

        public static async ETTask OnDeleteClickHandler(this DlgRoles self)
        {
            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Warning("请选择需要删除的角色");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                self.RefreshRoleItems();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            await ETTask.CompletedTask;
        }
    }
}