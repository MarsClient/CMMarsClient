using UnityEngine;
using System.Collections;

public class ButtonDialog : MonoBehaviour {

	public bool isYes;

	public DialogItem dialogItem;

	void OnClick ()
	{
		if (dialogItem.dialogContent != null)
		{
			if (dialogItem.dialogContent.dialogHandle != null)
			{
				dialogItem.dialogContent.dialogHandle (isYes);
			}
		}
		Dialog.instance.Close ();
	}
}
