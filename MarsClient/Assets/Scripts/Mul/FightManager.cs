using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightManager : MultiPlayer {

	public static FightManager instance;  void Awake () { instance = this; }

	private Dictionary<string, GameObject> monsters = new Dictionary<string, GameObject> ();

	void Start ()
	{
		base.Start ();
		gameObject.AddComponent <PoolManager>();
	}

	public override void LoadingDoneRoles ()
	{
		string[] test = new string[] {Constants.ENEMYS + "EE0001"};
		AssetLoader.Instance.DownloadAssetbundle (test, MonsterCallBack);
	}

	void MonsterCallBack (List<object> gos)
	{
		foreach (object o in gos)
		{
			//Debug.LogError (o.ToString());
			GameObject go = (GameObject) o;
			monsters.Add (go.name, go);

			NetSend.SendPlayersDone ();
			ScenesManager.instance.DelaySuccessLoading ();
		}
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
//		if (bundle.cmd == Command.PlayerDone)
//		{
//			foreach (Role role in Main.Instance.fight.team.roles)
//			{
//				if (role.roleId != Main.Instance.role.roleId && role.region == Main.Instance.fight.id)
//				{
//					AddNewPro (role);
//				}
//			}
//		}
	}

	void ProcessResultSync (Bundle bundle)
	{
//		if (bundle.cmd == Command.PlayerDone)
//		{
//			AddNewPro (bundle.role);//add new role
//		}
	}

	public TextAsset textAsset;


	#region follow is Test Code;
	private Fight fight;
	public void InitLocalData (string num)
	{
		if (fight == null)
		{
			fight = JsonConvert.DeserializeObject<Fight> (textAsset.text);
		}
		foreach (GameMonster mg in fight.gameMonsters[num])
		{
			GameObject prefab = monsters[mg.type];
			GameObject go = GameObject.Instantiate (prefab) as GameObject;
			go.transform.position = new Vector3 (mg.x, 0, mg.z);
			EnemyUnit eu = go.GetComponent<EnemyUnit>();
			if (eu != null)
			{
				eu.InitUI (mg);
			}
		}
	}
	#endregion
}
