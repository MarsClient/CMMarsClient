using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

[RequireComponent (typeof (UITabList))]
public class UIServerList : MonoBehaviour, ITabListener
{
	public GameObject prefab;
	public Transform mark;

	private UITabList tabButton;


	public Server currentServer = null;

	void Start () 
	{ 
		if (tabButton == null)
		{
			tabButton = GetComponent<UITabList>();
			tabButton.tabListener = this;
			tabButton.buttonPrefab = prefab;
		}
		mark.gameObject.SetActive (false);
	}
	
	public void Initialization (Server[] servers)
	{
		if (servers != null)
		{
			Start ();
			List<object> objs = new List<object> ();
			foreach (Server k in servers)
			{
				if (k.isSwitch == true) { objs.Add ((object)k); }
			}
			tabButton.refresh (objs);
		}
	}

	/*tabEvent*/
	public void TabInitialization(GameObject go, object obj)
	{
		ServerItem si = go.GetComponent<ServerItem>();
		Server s = (Server) obj;
		si.serverName.text = s.serverName;
		si.role.text = "(0)";//TODO:
		si.limit.text = Localization.Get ("server.limit" + s.limit);
	}
	public void TabOnClickMeesgae(object t, GameObject go, List<GameObject> btns)
	{
		currentServer = (Server)t;
		mark.gameObject.SetActive (true);
		mark.localPosition = go.transform.localPosition;
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
		}
	}
}
