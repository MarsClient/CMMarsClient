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
		pros[2] = Constants.PRO + ((int)PRO.WS).ToString ();

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
			else if (go.name == pros[2] ) p = PRO.WS;
			GameObject role = (GameObject) go;
			role.SetActive (false);
			if (p != PRO.NULL) PROS.Add (p, role);
		}
		UISceneLoading.instance.DelaySuccessLoading ();
		Debug.Log ("Done");
		AddNewPro (Main.Instance.role);
	}

	void AddNewPro (Role role)
	{
		GameObject go = null;
		PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
//		object o = ((object) role.profession);
//		o.
		PROS.TryGetValue (pro, out go);
		if (go != null)
		{
			GameObject r = ObjectPool.Instance.LoadObject (go);
			r.name = role.accountId.ToString ();
			CameraController.instance.initialize (r.transform);
			r.SetActive (true);
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
		PROS.Clear ();
	}

	void ProcessResults (Bundle bundle)
	{

	}

	void ProcessResultSync (Bundle bundle)
	{
	}

	public void OnDestroy ()
	{
		ObjectPool.Instance.Clear ();
	}
}
