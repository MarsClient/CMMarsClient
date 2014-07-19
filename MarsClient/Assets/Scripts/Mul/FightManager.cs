using UnityEngine;
using System.Collections;

public class FightManager : MultiPlayer {

	public static FightManager instance;  void Awake () { instance = this; }

	void Start ()
	{
		base.Start ();
		gameObject.AddComponent <PoolManager>();
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

	public TextAsset textAsset;


	#region follow is Test Code;
	public GameObject prefab;

	private Fight fight;
	public void InitLocalData (string num)
	{
		if (fight == null)
		{
			fight = JsonConvert.DeserializeObject<Fight> (textAsset.text);
		}
		foreach (GameMonster mg in fight.gameMonsters[num])
		{
			GameObject go = GameObject.Instantiate (prefab) as GameObject;
			go.transform.position = new Vector3 (mg.x, 0, mg.z);
		}
	}
	#endregion
}
