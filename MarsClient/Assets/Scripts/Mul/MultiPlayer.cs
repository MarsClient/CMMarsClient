using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MultiPlayer : MonoBehaviour {

	private Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();

	string[] pros = new string[3];

	void Start ()
	{
		pros[0] = Constants.PRO + ((int)PRO.ZS).ToString ();
		pros[1] = Constants.PRO + ((int)PRO.FS).ToString ();
		pros[2] = Constants.PRO + ((int)PRO.DZ).ToString ();

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
			GameObject role = (GameObject) go;
			role.SetActive (false);
			if (p != PRO.NULL) PROS.Add (p, role);
		}
		UISceneLoading.instance.DelaySuccessLoading ();
		Debug.Log ("Done" + Main.Instance.onlineRoles.Count);
		AddNewPro (Main.Instance.role);

		foreach (Role r in Main.Instance.onlineRoles)
		{
			AddNewPro (r);
		}
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
			GameObject r = ObjectPool.Instance.LoadObject (go);
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
