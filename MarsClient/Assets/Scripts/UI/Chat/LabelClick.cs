using UnityEngine;
using System.Collections;

public class LabelClick : MonoBehaviour {

	public UILabel lbl;
	public ChatInput chat;

	void OnClick ()
	{
		if (lbl == null) { lbl = GetComponent<UILabel>(); }
		if (lbl != null)
		{
			string url = lbl.GetUrlAtPosition(UICamera.lastHit.point);
			if (url != null)
			{
				string[] str = url.Split (',');
				chat.ClickInfo (str);
			}
		}
	}
}
