using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetRecv : MonoBehaviour {

	void OnEnable ()
	{
		PhotonClient.ProcessResults += ProcessResult;
	}

	void OnDisable ()
	{
		PhotonClient.ProcessResults -= ProcessResult;
	}

	void ProcessResult (Bundle bundle)
	{
		if (bundle.error == null)
		{
			if (bundle.cmd == Command.Handshake)
			{
				Main.Instance.sqliteVer = bundle.sqliteVer;
				StartCoroutine (GameData.Instance.reload ());
			}
			if (bundle.cmd == Command.Login)
			{
				Main.Instance.serverList = bundle.serverList;
				/*foreach (KeyValuePair<string, Server[]> kvp in bundle.serverList)
				{
					Debug.Log (kvp.Key + "___");
					foreach (Server s in kvp.Value)
					{
						Debug.LogError (s.serverName);
					}
				}*/
				Main.Instance.account = bundle.account;
			}
		}
	}
}
