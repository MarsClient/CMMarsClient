using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

[RequireComponent (typeof (UITabList))]
public class UITabServerList : MonoBehaviour, ITabListener {

	public UIServerList serverList;
	public GameObject prefab;	
	private UITabList tabButton;
	void Start () 
	{ 
		if (tabButton == null)
		{
			tabButton = GetComponent<UITabList>();
			tabButton.tabListener = this;
			tabButton.buttonPrefab = prefab;
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
	public void TabInitialization(GameObject go, object obj)
	{
		string key = (string)obj;
		go.GetComponentInChildren<UILabel>().text = key;
	}
	public void TabOnClickMeesgae(object t, GameObject go, List<GameObject> btns)
	{
		serverList.Initialization (Main.Instance.serverList[t.ToString ()]);
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
