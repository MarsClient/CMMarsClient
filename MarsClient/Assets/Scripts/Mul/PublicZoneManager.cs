using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PublicZoneManager : MultiPlayer {

	public static PublicZoneManager instance; void Awake () { instance = this; }

	void Start () 
	{
		UILoadPanel.LoadingPanel ();
		base.Start ();
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
		ScenesManager.instance.DelaySuccessLoading ();
	}

	public override void LoadingDoneRoles ()
	{
		AssetLoader.Instance.DownloadAssetbundle (GameData.Instance.getAllNpcsModel(), NpcCallBack);
		foreach (Role r in Main.Instance.onlineRoles)
		{
			AddNewPro (r);
		}
	}

	void OnEnable ()
	{
		PhotonClient.processResultSync += ProcessResultSync;
		//PhotonClient.processResultSync += ProcessResultSync;
	}
	
	void OnDisable ()
	{
		PhotonClient.processResultSync -= ProcessResultSync;
		//PhotonClient.processResultSync -= ProcessResultSync;
	}
	
	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.cmd == Command.AddNewPlayer)
		{
			AddNewPro (bundle.role);
		}
	}
	
	public void OnDestroy ()
	{
		PROS.Clear ();
//		ObjectPool.Instance.Clear ();
	}
}
