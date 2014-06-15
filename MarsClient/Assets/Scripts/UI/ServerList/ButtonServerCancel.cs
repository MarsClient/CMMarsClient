using UnityEngine;
using System.Collections;

public class ButtonServerCancel : MonoBehaviour {

	void OnClick ()
	{
		PanelsManager.Close ();
		PanelsManager.Show (PanelType.LoginRegister);
		PhotonClient.Instance.SetLoginServer ();
	}
}
