using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

[System.Serializable]
public class CreatMode : ITabListener
{
	#region ITabListener implementation

	public void TabInitialization (GameObject go, object obj)
	{
		//TODO:
	}

	public void TabOnClickMeesgae (object t, GameObject go, List<GameObject> btns)
	{
		foreach (GameObject g in btns)
		{
			bool isMine = (g == go);
			g.collider.enabled = !isMine;
			foreach (UISprite w in g.GetComponentsInChildren<UISprite>())
			{
				
				w.color = isMine ? Color.grey : Color.white;
			}
		}
		if (go.transform.parent == tabList1.transform) 
		{
			pro = (PRO) t;
			//Debug.Log ("PRO: " + (int)t);
		}
		if (go.transform.parent == tabList2.transform) 
		{
			sex = (int) t;
			//Debug.Log ("SEX: " + (int)t);
		}

	}

	#endregion

	public UIInput input;

	public UITabList tabList1;
	public UITabList tabList2;


	private int sex = 0;
	private PRO pro = PRO.ZS;

	public void Init ()
	{
		tabList1.tabListener = this;
		tabList1.refresh ();
		tabList2.tabListener = this;
		tabList2.refresh ();
	}

	private Error Ctrat ()
	{
		string inputStr = input.text; 

		Error e = null;
		if (inputStr != "")
		{
			if (input.text.Length < 2)
			{
				e = new Error ();
				e.message = "game.role.input.length";
			}
		}
		else
		{
			e = new Error ();
			e.message = "game.role.input.null";
		}
		return e;
	}
	public void StartCreat ()
	{
		Error e = this.Ctrat ();
		if (e != null)
		{
			new DialogContent ()
				.SetMessage (e.message)
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
		}
		else
		{
			//TODO:
			Role r = new Role ();
			r.accountId = Main.Instance.account.uniqueId;
			r.roleName = input.text;
			r.level = 1;
			r.sex = sex;
			r.profession = pro.ToString ();
			NetSend.SendCreatRole(r);
		}
	}
}

[System.Serializable]
public class RoleListMode
{
	public UILabel name;

}

public class RolePanel : MonoBehaviour 
{

	public CreatMode creatMode; 
	public RoleListMode roleListMode;

	void Start ()
	{
		creatMode.Init ();
	}

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
	}

	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
	}

	void CreatRoleOnClick ()
	{
		creatMode.StartCreat ();
	}

	void BackOnClick ()
	{
		PanelsManager.Close ();
		PanelsManager.Show (PanelType.ServerList);
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.CreatRole)
		{
			if (bundle.error == null)
			{
				new DialogContent ().SetMessage ("game.role.success").SetNoBtn ("game.dialog.no").ShowWaiting ();
			}
			if (bundle.error != null)
			{
				new DialogContent ().SetMessage (bundle.error.message).SetNoBtn ("game.dialog.no").ShowWaiting ();
			}
		}
	}
}
