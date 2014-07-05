using UnityEngine;
using System.Collections;

public class FightManager : MultiPlayer {


	void Start ()
	{
		base.Start ();
	}

	public override void LoadingDoneRoles ()
	{
		UISceneLoading.instance.DelaySuccessLoading ();
	}

	//public void
}
