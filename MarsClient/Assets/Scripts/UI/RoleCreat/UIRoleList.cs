using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TabButton;

public class UIRoleList : MonoBehaviour, ITabListener {

	public GameObject modelBg;
	private Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();
	string[] pros = new string[3];

	#region ITabListener implementation
	public void TabInitialization (GameObject go, object obj)
	{
		RoleItem roleItem = go.GetComponent<RoleItem>();
		Role role = (Role) obj;
		roleItem.SetData (role);
	}
	public void TabOnClickMeesgae (object t, GameObject go, List<GameObject> btns)
	{//Debug.LogError ("Bug");
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
			RoleItem roleItem = g.GetComponent<RoleItem>();

			roleItem.SetColor (isMine);
		}
		role = (Role) t;
		roleName.text = role.roleName;

		PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
		foreach (KeyValuePair<PRO, GameObject> kvp in PROS)
		{
			kvp.Value.SetActive (kvp.Key == pro);
		}

	}
	#endregion

	public GameObject prefab;
	public UILabel roleName;

	private UITabList tabButton;
	public Role role;
	
	void Start () 
	{
		if (tabButton == null)
		{
			tabButton = GetComponent<UITabList>();
			tabButton.tabListener = this;
			tabButton.buttonPrefab = prefab;
		}
	}

	void OnEnable ()
	{
		modelBg.SetActive (true);
		bool isActive = (Main.Instance == null || Main.Instance.roles == null || Main.Instance.roles.Count == 0);
		//SetCreatRole (isActive);
		if (isActive == false) { Initialization (Main.Instance.roles); }
	}

	void OnDisable ()
	{
//		Debug.Log ("Log");
		modelBg.SetActive (false);
	}

	public void Initialization (List<Role> roles)
	{
		if (roles != null)
		{
			new DialogContent ()
				.SetMessage ("server.link.success.after")
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();



			Start ();
			List<object> objs = new List<object> ();
			foreach (Role k in roles)
			{
				objs.Add ((object)k);
			}
			tabButton.refresh (objs);


			pros[0] = Constants.PRO + ((int)PRO.ZS).ToString ();
			pros[1] = Constants.PRO + ((int)PRO.FS).ToString ();
			pros[2] = Constants.PRO + ((int)PRO.DZ).ToString ();
			if (PROS.Count <= 0)
				AssetLoader.Instance.DownloadAssetbundle (pros, CallBack);
		}
	}

	void CallBack (List<object> gos)
	{
		foreach (object o in gos)
		{
			GameObject go = (GameObject) o;
			PRO p = PRO.NULL;
			if (go.name == pros[0] ) p = PRO.ZS;
			else if (go.name == pros[1] ) p = PRO.FS;
			else if (go.name == pros[2] ) p = PRO.DZ;
			GameObject _role = NGUITools.AddChild (modelBg, go);
			
			_role.SetActive (false);
			if (p != PRO.NULL) PROS.Add (p, _role);
		}
		//		UISceneLoading.instance.DelaySuccessLoading ();
		Debug.Log ("Done");
		Dialog.instance.TweenClose ();
		PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
		PROS[pro].SetActive (true);
	}
}
