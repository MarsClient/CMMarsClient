using UnityEngine;
using System.Collections;

public class ButtonServerSelect : MonoBehaviour {

	public UIServerList serverList;

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
	}
	
	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
	}

	void ProcessResults (Bundle bundle)
	{

		if (bundle.cmd == Command.LinkServer)
		{
			new DialogContent ()
				.SetMessage("server.link.success")
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
			serverList.currentServer.accountId = Main.Instance.account.uniqueId;
			NetSend.SendServerSelect (serverList.currentServer);
		}
		if (bundle.cmd == Command.ServerSelect)
		{
			Debug.Log (bundle.error);
			if (bundle.error == null)
			{
				Dialog.instance.TweenClose ();
				PanelsManager.Close ();
				PanelsManager.Show (PanelType.CreatRole);
			}
			else
			{
				new DialogContent ()
					.SetMessage(bundle.error.message)
						.SetNoBtn ("game.dialog.no")
						.ShowWaiting ();
			}
		}
	}

	void OnClick ()
	{
		if (serverList.currentServer != null)
		{
			PhotonClient.Instance.SetGameServer (serverList.currentServer.ip);
			new DialogContent ()
				.SetMessage("server.linking")
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
		}
	}
}
