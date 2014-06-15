using UnityEngine;
using System.Collections;

public class ButtonServerCancel : MonoBehaviour {

	void OnClick ()
	{
		PanelsManager.Instance.Close ();
		PanelsManager.Instance.Show (PanelType.LoginRegister);
		PhotonClient.Instance.SetLoginServer ();
	}
}
