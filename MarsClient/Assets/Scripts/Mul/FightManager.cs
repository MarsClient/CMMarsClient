using UnityEngine;
using System.Collections;

public class FightManager : MultiPlayer {

	public static FightManager instance;  void Awake () { instance = this; }

	void Start ()
	{
		base.Start ();
	}

	public override void LoadingDoneRoles ()
	{
		NetSend.SendPlayersDone ();
		UISceneLoading.instance.DelaySuccessLoading ();
	}

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
		PhotonClient.processResultSync += ProcessResultSync;
	}
	
	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
		PhotonClient.processResultSync -= ProcessResultSync;
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.PlayerDone)
		{
			foreach (Role role in Main.Instance.fight.team.roles)
			{
				if (role.roleId != Main.Instance.role.roleId && role.region == Main.Instance.fight.id)
				{
					AddNewPro (role);
				}
			}
		}
	}

	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.cmd == Command.PlayerDone)
		{
			AddNewPro (bundle.role);//add new role
		}
	}
}
