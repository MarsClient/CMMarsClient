using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TabButton;

[System.Serializable]
public class CreatMode : ITabListener
{
	public GameObject modelBg;
	private Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();
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
			//Debug.Log (w.name);
			g.GetComponent<UISprite>().color = isMine ? Color.grey : Color.white;
		}
		if (go.transform.parent == tabList1.transform) 
		{
			pro = (PRO) ((int) t + Constants.START_ID);
			foreach (KeyValuePair<PRO, GameObject> kvp in PROS)
			{
				kvp.Value.SetActive (kvp.Key == pro);
			}
			//Debug.Log ("PRO: " + (int)t);
		}
		if (go.transform.parent == tabList2.transform) 
		{
			sex = (int) t;
			//Debug.Log ("SEX: " + (int)t);
		}

	}

	#endregion
	public GameObject mainObj;
	public UIInput input;

	public UITabList tabList1;
	public UITabList tabList2;


	private int sex = 0;
	private PRO pro = PRO.ZS;


	string[] pros = new string[3];
	public void Init ()
	{
		new DialogContent ()
			.SetMessage ("server.link.success.after")
				.SetNoBtn ("game.dialog.no")
				.ShowWaiting ();


		if (tabList1.tabListener == null)
			tabList1.tabListener = this;
		tabList1.refresh ();
		if (tabList2.tabListener == null)
			tabList2.tabListener = this;
		tabList2.refresh ();

		pros[0] = Constants.PRO + ((int)PRO.ZS).ToString () + Constants.PRO_CREAT;
		pros[1] = Constants.PRO + ((int)PRO.FS).ToString () + Constants.PRO_CREAT;
		pros[2] = Constants.PRO + ((int)PRO.DZ).ToString () + Constants.PRO_CREAT;
		
		AssetLoader.Instance.DownloadAssetbundle (pros, CallBack);
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
			GameObject role = NGUITools.AddChild (modelBg, go);

			role.SetActive (false);
			if (p != PRO.NULL) PROS.Add (p, role);
		}
//		UISceneLoading.instance.DelaySuccessLoading ();
//		Debug.Log ("Done");
		Dialog.instance.TweenClose ();
		PROS[pro].SetActive (true);
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
	public GameObject mainObj;
}

public class RolePanel : MonoBehaviour 
{
	public CreatMode creatMode; 
	public RoleListMode roleListMode;
	public UIRoleList roleList;

	private bool isCreatInit = false;
	void Start ()
	{
		bool isActive = (Main.Instance == null || Main.Instance.roles == null || Main.Instance.roles.Count == 0);
		SetCreatRole (isActive);
		//creatMode.Init ();
	}

	void OnEnable ()
	{
		//if (isActive == false) { roleList.Initialization (Main.Instance.roles); }

		PhotonClient.processResults += ProcessResults;
	}

	void SetCreatRole (bool isActive)
	{
		creatMode.mainObj.SetActive (isActive);
		if (isActive == true && isCreatInit == false)
		{
			isCreatInit = true;
			creatMode.Init ();
		}
		roleListMode.mainObj.SetActive (!isActive);
	}

	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
	}

	void CreatRoleOnClick ()
	{
		creatMode.StartCreat ();
	}

	void BackToRoleList ()
	{
		if (Main.Instance.roles != null && Main.Instance.roles.Count > 0)
		{
			SetCreatRole (false);
			creatMode.modelBg.SetActive (false);
		}
		else
		{
			BackOnClick ();
		}
	}

	void BackOnClick ()
	{
		PanelsManager.Close ();
		PanelsManager.Show (PanelType.ServerList);
		NetSend.SendAbortDiscount ();
	}

	void CreatOnClick ()
	{
		creatMode.modelBg.SetActive (true);
		SetCreatRole (true);
	}

	void EnterGameOnClick ()
	{
		NetSend.SendEnterGame (roleList.role);
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.CreatRole)
		{
			if (bundle.error == null)
			{
				new DialogContent ().SetMessage ("game.role.success").SetNoBtn ("game.dialog.no").ShowWaiting ();
				Dialog.instance.TweenClose ();
				if (Main.Instance.roles.Contains (bundle.role) == false)
				{ 
					//Debug.Log (bundle.role.roleId);
					Main.Instance.roles.Add (bundle.role);
				}
				BackToRoleList ();
			}
			if (bundle.error != null)
			{
				new DialogContent ().SetMessage (bundle.error.message).SetNoBtn ("game.dialog.no").ShowWaiting ();
			}
		}
	}
}
