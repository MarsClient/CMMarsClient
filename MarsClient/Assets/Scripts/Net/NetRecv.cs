using UnityEngine;
using System.Collections;

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
				Main.Instance.account = bundle.account;
			}
		}
	}
}
