using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region Dialog Data
public enum DiaglogType
{
	waiting = 0,
	normal
}
public class DialogContent
{
	/**all delegate*/
	public delegate void DialogHandle (bool yes);
	public DialogHandle dialogHandle;
	
	/** all keys */
	public string message;
	public string yesStr;
	public string noStr;
	
	
	/**all method*/
	public DialogContent () {}
	public DialogContent SetMessage (string message, params string[] objs ) { this.message = string.Format (Localization.Get (message), objs); return this; }
	public DialogContent SetYesBtn (string yesStr, params string[] objs) { this.yesStr = string.Format (Localization.Get (yesStr), objs); return this; }
	public DialogContent SetNoBtn (string noStr, params string[] objs) { this.noStr = string.Format (Localization.Get (noStr), objs); return this; }
	public DialogContent SetDelegateBtn (DialogHandle dialogHandle) { this.dialogHandle = dialogHandle; return this; }
	
	public void Show () { Dialog.instance.Show (this); }
	public void ShowWaiting () { Dialog.instance.ShowWaiting (this); }
}
#endregion


public class Dialog : MonoBehaviour {
	
	public static Dialog instance;
	void Awake () {if (instance == null) { instance = this; DontDestroyOnLoad (gameObject); } else if (instance != this) Destroy (gameObject); }

	public DialogItem dialogItem;

	public void Show (DialogContent dc)//normal
	{
		ShowActiveState ();
		dialogItem.Refresh (dc, DiaglogType.normal);
		//dialogItem.Refresh (dialogItem);
	}

	public void ShowWaiting (DialogContent dc)//normal
	{
		ShowActiveState ();
		dialogItem.Refresh (dc, DiaglogType.waiting);
	}

	private void ShowActiveState ()
	{
		dialogItem.panel.alpha = 1.0f;
		dialogItem.gameObject.SetActive (true);
	}

	public void Close ()
	{
		dialogItem.gameObject.SetActive (false);
	}

	public void TweenClose ()
	{
		Close ();
//		TweenAlpha ta = TweenAlpha.Begin (dialogItem.gameObject, 0.25f, 0);
//		ta.AddOnFinished (CloseDialog);
	}

	void CloseDialog ()
	{
		dialogItem.gameObject.SetActive (false);
	}

}
