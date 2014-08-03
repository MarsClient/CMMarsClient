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
			if (PROS.Count <= 0)
			{
				new DialogContent ()
					.SetMessage ("server.link.success.after")
						.SetNoBtn ("game.dialog.no")
						.ShowWaiting ();
			}


			Start ();
			List<object> objs = new List<object> ();
			foreach (Role k in roles)
			{
				objs.Add ((object)k);
			}
			tabButton.refresh (objs);

			AssetLoader.Instance.LoadingGameObjectWithDontDestroy ((string[] m_Str)=>
			{
				Dialog.instance.TweenClose ();

				if (role != null)
				{
					foreach (string str in m_Str)
					{
						GameObject go = AssetLoader.Instance.TryGetDontDestroyObject (str);
						GameObject _role = NGUITools.AddChild (modelBg, go);

						PRO p = PRO.NULL;
						if (str.Contains (((int)PRO.ZS).ToString())) p = PRO.ZS;
						else if (str.Contains (((int)PRO.DZ).ToString())) p = PRO.DZ;
						else if (str.Contains (((int)PRO.FS).ToString())) p = PRO.FS;

						_role.SetActive (false);
						PROS.Add (p, _role);
					}

					PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
					if (pro != PRO.NULL)
					{
						PROS[pro].SetActive (true);
					}
				}
			});
		}
	}	
}
