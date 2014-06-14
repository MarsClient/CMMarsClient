using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

public class UIserverList : MonoBehaviour {


	private UITabButton tabButton;

	void OnEnable () 
	{ 
		if (tabButton == null)
			tabButton = GetComponentInChildren<UITabButton>();
		tabButton.buttonTabMeesgae += ButtonServerEvent;
		Init ();
	}

	void OnDisable () 
	{ 
		tabButton.buttonTabMeesgae -= ButtonServerEvent; 
	}

	void Init ()
	{
		if (Main.Instance.serverList != null)
		{
			List<object> objs = new List<object> ();
			foreach (string k in Main.Instance.serverList.Keys)
			{
				objs.Add ((object)k);
			}
			tabButton.refresh (objs, ButtonServerInit);
		}
	}

	void ButtonServerInit(UILabel label, object obj)
	{
		string key = (string)obj;
		label.text = key;
	}
	void ButtonServerEvent (object obj, GameObject go, List<GameObject> btns)
	{
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
//			Debug.LogError (isMine + "___" + go.name + "___" + g.name);
			foreach (UIWidget w in g.GetComponentsInChildren<UIWidget>())
			{

				w.color = isMine ? Color.grey : Color.white;
			}
		}
	}
}
