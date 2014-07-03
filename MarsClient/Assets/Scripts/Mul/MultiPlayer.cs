using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MultiPlayer : MonoBehaviour {

	private Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();

	string[] pros = new string[3];

	void Start ()
	{
		UILoadPanel.LoadingPanel ();


		pros[0] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.ZS).ToString ();
		pros[1] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.FS).ToString ();
		pros[2] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.DZ).ToString ();

		AssetLoader.Instance.DownloadAssetbundle (pros, CallBack);


	}

	void CallBack (List<object> gos)
	{
		foreach (object o in gos)
		{
			GameObject go = (GameObject) o;
			PRO p = PRO.NULL;
			if (go.name == pros[0].Split ('/')[1] ) p = PRO.ZS;
			else if (go.name == pros[1].Split ('/')[1] ) p = PRO.FS;
			else if (go.name == pros[2].Split ('/')[1] ) p = PRO.DZ;
			GameObject role = (GameObject) go;
			role.SetActive (false);
			if (p != PRO.NULL) PROS.Add (p, role);
		}
		AssetLoader.Instance.DownloadAssetbundle (GameData.Instance.getAllNpcsModel(), NpcCallBack);
		Debug.Log ("Done" + Main.Instance.onlineRoles.Count);
		AddNewPro (Main.Instance.role);

		foreach (Role r in Main.Instance.onlineRoles)
		{
			AddNewPro (r);
		}
	}

	void NpcCallBack (List<object> gos)
	{
		GameObject npcManager = new GameObject ("NpcManager");
		npcManager.AddComponent <NpcManager>();
		foreach (object o in gos)
		{
			//Debug.LogError (o.ToString());
			GameObject go = (GameObject) o;
			GameNPC npc = GameData.Instance.getNpcByModel (go.name);
			if (npc != null)
			{
				GameObject r = GameObject.Instantiate (go) as GameObject;
				r.transform.parent = npcManager.transform;
				NpcController npcController = r.GetComponent<NpcController>();

				npcController.Refresh (npc);
			}
		}
		UISceneLoading.instance.DelaySuccessLoading ();
	}

	void AddNewPro (Role role)
	{
		if (role == null)
			return;
		GameObject go = null;
		PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
//		object o = ((object) role.profession);
//		o.
		PROS.TryGetValue (pro, out go);
		if (go != null)
		{
			GameObject r = GameObject.Instantiate (go) as GameObject;
			r.name = role.accountId.ToString ();
			PlayerUnit hit = r.GetComponent <PlayerUnit>();
			hit.DataRefresh (role);

			r.SetActive (true);

			if (role.roleId != Main.Instance.role.roleId)
			{
				hit.RefreshMulPlayerState (role);
				Destroy (r.GetComponent<AiPlayer>());
				Destroy (r.GetComponent<AiInput>());
				return;
			}
			CameraController.instance.initialize (r.transform);
		}
	}

	void AddNpc (GameNPC npc)
	{
		if (npc == null) return;

	}

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
		//PhotonClient.processResultSync += ProcessResultSync;
	}

	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
		//PhotonClient.processResultSync -= ProcessResultSync;
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.AddNewPlayer)
		{
			AddNewPro (bundle.role);
		}
	}
//
//	void ProcessResultSync (Bundle bundle)
//	{
//	}

	public void OnDestroy ()
	{
		PROS.Clear ();
		ObjectPool.Instance.Clear ();
	}
}
