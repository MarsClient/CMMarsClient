using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

public class UIRoleList : MonoBehaviour, ITabListener {

	#region ITabListener implementation
	public void TabInitialization (GameObject go, object obj)
	{
		RoleItem roleItem = go.GetComponent<RoleItem>();
		Role role = (Role) obj;
		roleItem.SetData (role);
	}
	public void TabOnClickMeesgae (object t, GameObject go, List<GameObject> btns)
	{
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
			RoleItem roleItem = g.GetComponent<RoleItem>();

			roleItem.SetColor (isMine);
//			if (isMine == true) { roleItem.ableClick ();  }
//			else roleItem.disableClick ();
		}
	}
	#endregion

	public GameObject prefab;

	private UITabList tabButton;
	private Role role;
	
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
		bool isActive = (Main.Instance == null || Main.Instance.roles == null || Main.Instance.roles.Count == 0);
		//SetCreatRole (isActive);
		if (isActive == false) { Initialization (Main.Instance.roles); }
	}

	public void Initialization (List<Role> roles)
	{
		if (roles != null)
		{
			Start ();
			List<object> objs = new List<object> ();
			foreach (Role k in roles)
			{
				objs.Add ((object)k);
			}
			tabButton.refresh (objs);
		}
	}
}
