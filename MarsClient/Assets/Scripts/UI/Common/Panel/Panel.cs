using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : MonoBehaviour 
{
	public static Dictionary<PanelType, Panel> panels = new Dictionary<PanelType, Panel> ();
	public bool isNeedClose = true;
	public PanelType panelType;

	void Start ()
	{
		panels.Add (panelType, this);
		if (isNeedClose) Close ();
	}

 	public void Show ()
	{
		gameObject.SetActive (true);
	}

	public void Close ()
	{
		gameObject.SetActive (false);
	}

	void OnDestroy ()
	{
		panels.Remove (panelType);
		//Debug.Log ("NetTest");
	}
}
