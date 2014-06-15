using UnityEngine;
using System.Collections;

public class ButtonPanelClose : MonoBehaviour {

	void OnClick ()
	{
		PanelsManager.Instance.Close ();
	}
}
