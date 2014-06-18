using UnityEngine;
using System.Collections;

[System.Serializable]
public class CreatMode
{
	public UIInput input;
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
			r.profession = PRO.ZS.ToString ();
			NetSend.SendCreatRole(r);
		}
	}
}

public class RolePanel : MonoBehaviour 
{

	public CreatMode creatMode; 

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
