using UnityEngine;
using System.Collections;

public class UILoadPanel : MonoBehaviour {

	public string[] panels;
	private GameObject anchor;

	void Start () 
	{
		//creat root;
		GameObject res_go =  Resources.Load (Constants.UI + "UI Root (Hud)", typeof (GameObject)) as GameObject;
		GameObject root = GameObject.Instantiate (res_go) as GameObject;
		root.transform.position = new Vector3 (0,0,-10000);
		UIHudManager hudManager = root.GetComponent<UIHudManager>();
		anchor = hudManager.anchor;
		CreatAllPanel ();
	}

	void CreatAllPanel ()
	{
		foreach (string p in panels)
		{
			GameObject res_go =  Resources.Load (Constants.UI + p, typeof (GameObject)) as GameObject;
			GameObject panel = NGUITools.AddChild (anchor, res_go);
		}
	}


	public static void LoadingPanel ()
	{
		GameObject go = Resources.Load (Constants.UI + "PublicZone", typeof (GameObject)) as GameObject;
		GameObject.Instantiate (go);
//		Debug.Log (go);
	}
}
