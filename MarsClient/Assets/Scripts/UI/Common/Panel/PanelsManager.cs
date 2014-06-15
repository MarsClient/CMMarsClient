using UnityEngine;
using System.Collections;

public enum PanelType
{
	LoginRegister,
	ServerList,
	CreatRole
}

public class PanelsManager
{
	public readonly static PanelsManager Instance = new PanelsManager ();

	public static void Show (PanelType type)
	{
		Panel p = null;
		Panel.panels.TryGetValue (type, out p);
		if (p != null)
		{
			p.Show ();
		}
	}

	public static void Close ()
	{
		foreach (Panel p in Panel.panels.Values) 
		{
			p.Close ();
		}
	}
}
