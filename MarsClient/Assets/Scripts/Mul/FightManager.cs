using UnityEngine;
using System.Collections;

public class FightManager : MultiPlayer {


	void Start ()
	{
		base.Start ();
	}

	public override void LoadingDoneRoles ()
	{
		NetSend.SendPlayersDone ();
		//UISceneLoading.instance.DelaySuccessLoading ();
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

	}

	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.cmd == Command.PlayerDone)
		{
			UISceneLoading.instance.DelaySuccessLoading ();
		}
	}
}
