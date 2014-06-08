using UnityEngine;
using System.Collections;

public class DialogItem : MonoBehaviour {

	public float btnOffset;
	public float btnMidden;

	public UILabel message;
	public GameObject yesBtn;
	public GameObject noBtn;

	public DialogContent dialogContent;

	void Init () { message.text = ""; yesBtn.SetActive (false); noBtn.SetActive (false); }
	public void Refresh (DialogContent dc, DiaglogType dt)
	{
		Init ();
		dialogContent = dc;
		if (dt == DiaglogType.normal)
		{
			yesBtn.SetActive (true);
			noBtn.SetActive (true);
			message.text = dc.message;
			yesBtn.GetComponentInChildren<UILabel>().text = dc.yesStr;
			noBtn.GetComponentInChildren<UILabel>().text = dc.noStr;
			SetBtnPos (yesBtn, -btnOffset);
			SetBtnPos (noBtn, btnOffset);
		}
	}

	void SetBtnPos (GameObject btn, float offset)
	{
		Vector3 pos = btn.transform.localPosition;
		pos.x = offset;
		btn.transform.localPosition = pos;
	}
}
