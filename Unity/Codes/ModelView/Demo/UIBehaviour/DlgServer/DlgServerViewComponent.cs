
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgServerViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.RectTransform EGBackGroundRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EGBackGroundRectTransform == null )
     			{
		    		this.m_EGBackGroundRectTransform = UIFindHelper.FindDeepChild<UnityEngine.RectTransform>(this.uiTransform.gameObject,"EGBackGround");
     			}
     			return this.m_EGBackGroundRectTransform;
     		}
     	}

		public UnityEngine.UI.LoopVerticalScrollRect E_ServerListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ServerListLoopVerticalScrollRect == null )
     			{
		    		this.m_E_ServerListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"EGBackGround/E_ServerList");
     			}
     			return this.m_E_ServerListLoopVerticalScrollRect;
     		}
     	}

		public UnityEngine.UI.Button E_EnterServerButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterServerButton == null )
     			{
		    		this.m_E_EnterServerButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/E_EnterServer");
     			}
     			return this.m_E_EnterServerButton;
     		}
     	}

		public UnityEngine.UI.Image E_EnterServerImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterServerImage == null )
     			{
		    		this.m_E_EnterServerImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/E_EnterServer");
     			}
     			return this.m_E_EnterServerImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EGBackGroundRectTransform = null;
			this.m_E_ServerListLoopVerticalScrollRect = null;
			this.m_E_EnterServerButton = null;
			this.m_E_EnterServerImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.RectTransform m_EGBackGroundRectTransform = null;
		private UnityEngine.UI.LoopVerticalScrollRect m_E_ServerListLoopVerticalScrollRect = null;
		private UnityEngine.UI.Button m_E_EnterServerButton = null;
		private UnityEngine.UI.Image m_E_EnterServerImage = null;
		public Transform uiTransform = null;
	}
}
