using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : MonoBehaviour 
{
	public static Dictionary<PanelType, Panel> panels = new Dictionary<PanelType, Panel> ();
	public bool isInitNeedClose = true;
	public PanelType panelType;

	public delegate void PanelStartEvent (Panel panel);

	void Start ()
	{
		panels.Add (panelType, this);
		if (isInitNeedClose) Close ();
	}
	public void Show (PanelStartEvent panelStartEvent)
	{
		gameObject.SetActive (true);
		if (panelStartEvent != null)
		{
			panelStartEvent (this);
		}
	}

	public void Close ()
	{
		gameObject.SetActive (false);
	}

	void OnDestroy ()
	{
		panels.Remove (panelType);
	}
}
