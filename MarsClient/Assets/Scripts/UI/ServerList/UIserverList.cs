using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

public class UIserverList : MonoBehaviour, ITabListener {


	private UITabButton tabButton;

	void Start () 
	{ 
		if (tabButton == null)
		{
			tabButton = GetComponentInChildren<UITabButton>();
			tabButton.tabListener = this;
			Initialization ();
		}
	}

	void Initialization ()
	{
		if (Main.Instance.serverList != null)
		{
			List<object> objs = new List<object> ();
			foreach (string k in Main.Instance.serverList.Keys)
			{
				objs.Add ((object)k);
			}
			tabButton.refresh (objs);
		}
	}

	/*tabEvent*/
	public void TabInitialization(UILabel label, object obj)
	{
		string key = (string)obj;
		label.text = key;
	}
	public void TabOnClickMeesgae(object t, GameObject go, List<GameObject> btns)
	{
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
			foreach (UIWidget w in g.GetComponentsInChildren<UIWidget>())
			{
				
				w.color = isMine ? Color.grey : Color.white;
			}
		}
	}
}
