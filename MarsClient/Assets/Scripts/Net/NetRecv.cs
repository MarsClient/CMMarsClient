using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetRecv : MonoBehaviour {

	private bool isInit = true;
	public void ProcessResult (Bundle bundle)
	{
		if (bundle.error == null)
		{
			if (bundle.cmd == Command.Handshake && isInit)
			{
				isInit = false;
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
			if (bundle.cmd == Command.ServerSelect)
			{
				Main.Instance.server = bundle.server;
				Main.Instance.roles = bundle.roles;
			}
			if (bundle.cmd == Command.EnterGame)
			{
				Main.Instance.role = bundle.role;
				UISceneLoading.LoadingScnens ("PublicZone");
			}
		}
		else
		{
			if (bundle.cmd == Command.NetError)
			{
				new DialogContent ()
					.SetMessage(bundle.error.message)
						.SetNoBtn ("game.dialog.yes")
						.SetDelegateBtn (DialogIndexDelegate)
						.ShowWaiting ();

			}
		}
	}

	void DialogIndexDelegate (bool isBy)
	{
		PanelsManager.Close ();
		PanelsManager.Show (PanelType.ServerList);
	}

}
