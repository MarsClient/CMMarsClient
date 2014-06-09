﻿using UnityEngine;
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
	public DialogContent SetMessage (string message) { this.message = Localization.instance.Get (message); return this; }
	public DialogContent SetYesBtn (string yesStr) { this.yesStr = Localization.instance.Get (yesStr); return this; }
	public DialogContent SetNoBtn (string noStr) { this.noStr = Localization.instance.Get (noStr); return this; }
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
		dialogItem.gameObject.SetActive (true);
		dialogItem.Refresh (dc, DiaglogType.normal);
		//dialogItem.Refresh (dialogItem);
	}

	public void ShowWaiting (DialogContent dc)//normal
	{
		dialogItem.gameObject.SetActive (true);
		dialogItem.Refresh (dc, DiaglogType.waiting);
	}

	public void Close ()
	{
		dialogItem.gameObject.SetActive (false);
	}

}
