using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class MultiPlayer : MonoBehaviour {

	public void Start ()
	{
		CallBack ();
	}

	void CallBack ()
	{
		AddNewPro (Main.Instance.role);
		LoadingDoneRoles ();
	}

	protected void AddNewPro (Role role)
	{
		if (role == null)
			return;
		GameObject go = null;
		PRO pro = (PRO) Enum.Parse (typeof (PRO), role.profession);
		string key = Constants.RO_STRING + ((int) pro).ToString();
		Debug.Log (key);
		go = AssetLoader.Instance.TryGetDontDestroyObject (key);
		if (go != null)
		{
			GameObject r = GameObject.Instantiate (go) as GameObject;
			PlayerUnit hit = r.GetComponent <PlayerUnit>();
			hit.DataRefresh (role);

			r.SetActive (true);

			if (role.roleId != Main.Instance.role.roleId)
			{
				hit.RefreshMulPlayerState (role);
				return;
			}
			r.AddComponent<AiInput>();
			CameraController.instance.initialize (r.transform, CameraType.Follow);
		}
	}

	public abstract void LoadingDoneRoles ();
}
