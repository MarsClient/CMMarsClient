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
			Server server = new Server ();
			server.accountId = Main.Instance.account.uniqueId;
			server.serverId = serverList.currentServer.serverId;
			//serverList.currentServer.accountId = Main.Instance.account.uniqueId;
			Main.Instance.server = server;
			NetSend.SendServerSelect (server);
		}
		if (bundle.cmd == Command.ServerSelect)
		{
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
				PhotonClient.Instance.PeerDiscount ();
			}
		}
	}

	void OnClick ()
	{
		if (serverList.currentServer != null)
		{
			PhotonClient.Instance.LoadingGameServer (serverList.currentServer.ip);
			new DialogContent ()
				.SetMessage("server.linking")
					.SetNoBtn ("game.dialog.no")
					.ShowWaiting ();
		}
	}
}
