﻿
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[EnableMethod]
	public  class Scroll_Item_role : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_Item_role BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Button E_RoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_RoleButton == null )
     				{
		    			this.m_E_RoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Role");
     				}
     				return this.m_E_RoleButton;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Role");
     			}
     		}
     	}

		public UnityEngine.UI.Image E_RoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_RoleImage == null )
     				{
		    			this.m_E_RoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Role");
     				}
     				return this.m_E_RoleImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Role");
     			}
     		}
     	}

		public UnityEngine.UI.Text E_RoleNameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_RoleNameText == null )
     				{
		    			this.m_E_RoleNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_RoleName");
     				}
     				return this.m_E_RoleNameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_RoleName");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_RoleButton = null;
			this.m_E_RoleImage = null;
			this.m_E_RoleNameText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_E_RoleButton = null;
		private UnityEngine.UI.Image m_E_RoleImage = null;
		private UnityEngine.UI.Text m_E_RoleNameText = null;
		public Transform uiTransform = null;
	}
}
