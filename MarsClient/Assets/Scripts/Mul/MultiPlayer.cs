using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class MultiPlayer : MonoBehaviour {

	protected Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();

	string[] pros = new string[3];

	public void Start ()
	{
		pros[0] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.ZS).ToString ();
		pros[1] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.FS).ToString ();
		pros[2] = Constants.PRO + Constants.PRO.Replace ("/", "") + ((int)PRO.DZ).ToString ();
		if (AssetLoader.Instance != null)
			AssetLoader.Instance.DownloadAssetbundle (pros, CallBack, true);


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

//		Debug.Log ("Done" + Main.Instance.onlineRoles.Count);
		AddNewPro (Main.Instance.role);
		//Debug.Log (UISceneLoading.currentLoadName);

		LoadingDoneRoles ();
	}



	protected void AddNewPro (Role role)
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
			//r.name = role.accountId.ToString ();
			PlayerUnit hit = r.GetComponent <PlayerUnit>();
			hit.DataRefresh (role);

			r.SetActive (true);

			if (role.roleId != Main.Instance.role.roleId)
			{
				hit.RefreshMulPlayerState (role);
				//Destroy (r.GetComponent<AiPlayer>());
				//Destroy (r.GetComponent<AiInput>());
				return;
			}
			r.AddComponent<AiInput>();
			CameraController.instance.initialize (r.transform, CameraType.Follow);
		}
	}

	public abstract void LoadingDoneRoles ();
}
