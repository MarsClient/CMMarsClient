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

	public bool isNormal = false;
	public bool isWait = false;
	
	
	/**all method*/
	public DialogContent () {}
	public DialogContent SetMessage (string message, params string[] objs ) { this.message = string.Format (Localization.Get (message), objs); return this; }
	public DialogContent SetYesBtn (string yesStr, params string[] objs) { this.yesStr = string.Format (Localization.Get (yesStr), objs); return this; }
	public DialogContent SetNoBtn (string noStr, params string[] objs) { this.noStr = string.Format (Localization.Get (noStr), objs); return this; }
	public DialogContent SetDelegateBtn (DialogHandle dialogHandle) { this.dialogHandle = dialogHandle; return this; }
	
	public void Show () { isNormal = true; Dialog.instance.PushDialogContent (this); }
	public void ShowWaiting () { isWait = true; Dialog.instance.PushDialogContent (this); }
}
#endregion


public class Dialog : MonoBehaviour {
	
	public static Dialog instance;
	void Awake () {if (instance == null) { instance = this; DontDestroyOnLoad (gameObject); } else if (instance != this) Destroy (gameObject); }

	public DialogItem dialogItem;

	private Queue<DialogContent> dialogContents = new Queue<DialogContent> ();//queue
	private bool isQueue { get { return dialogContents.Count > 0; } }
	private bool inUsing { get { return dialogItem.gameObject.activeSelf; } }

	/*public void Show (DialogContent dc)
	{
		ShowActiveState ();
		dialogItem.Refresh (dc, DiaglogType.normal);
	}

	public void ShowWaiting (DialogContent dc)
	{
		ShowActiveState ();
		dialogItem.Refresh (dc, DiaglogType.waiting);
	}*/

	public void PushDialogContent (DialogContent dc)
	{
		//dialogContents.Enqueue (dc);
		//if (inUsing == false)
		{

			Show (dc/*dialogContents.Dequeue ()*/);
		}
	}

	public void Show (DialogContent dc)
	{
		ShowActiveState ();
		if (dc.isNormal)
		{
			dialogItem.Refresh (dc, DiaglogType.normal);
		}
		else if (dc.isWait)
		{
			dialogItem.Refresh (dc, DiaglogType.waiting);
		}
	}

	private void ShowActiveState ()
	{
		dialogItem.panel.alpha = 1.0f;
		dialogItem.gameObject.SetActive (true);
	}

	public void Close ()
	{
		dialogItem.gameObject.SetActive (false);
//		if (dialogContents.Count > 0)
//		{
//			Show (dialogContents.Dequeue ());
//		}
	}

	public void TweenClose ()
	{
		Close ();
	}

	void CloseDialog ()
	{
		dialogItem.gameObject.SetActive (false);
	}

}
