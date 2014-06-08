using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {
	
	private static Dialog instance;
	void Awake () {if (instance == null) { instance = this; DontDestroyOnLoad (gameObject); } else if (instance != this) Destroy (gameObject); }

#region Dialog Data
	public enum DiaglogType
	{
		waiting,
		normal
	}
	public class DialogContent
	{
		/**all delegate*/
		public delegate void DialogHandle ();
		public DialogHandle dialogHandle;

		/** all keys */
		public string message;
		public string yesStr;
		public string noStr;


		/**all method*/
		public DialogContent () {}
		public DialogContent SetMessage (string message) { this.message = message; return this; }
		public DialogContent SetYesBtn (string yesStr) { this.yesStr = yesStr; return this; }
		public DialogContent SetNoBtn (string noStr) { this.noStr = noStr; return this; }
		public DialogContent SetNoBtn (DialogHandle dialogHandle) { this.dialogHandle = dialogHandle; return this; }

		public void Show () { Dialog.instance.Show (this); }
	}
#endregion

	void Start ()
	{

	}

	public void Show (DialogContent dialogContent)
	{

	}
	
//	void OnGUI ()
//	{
//		if (GUILayout.Button ("hahahahhahahah"))
//		{
//
//		}
//	}
}
