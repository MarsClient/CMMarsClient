﻿using UnityEngine;
using System.Collections;

public class ButtonServerSelect : MonoBehaviour {

	public UIServerList serverList;

	public GameObject spin;

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
		if (bundle.error == null)
		{
			if (bundle.cmd == Command.LinkServer)
			{
				new DialogContent ()
					.SetMessage("server.link.success")
						.SetNoBtn ("game.dialog.no")
						.ShowWaiting ();
				Dialog.instance.TweenClose ();
				NetSend.SendServerSelect (serverList.currentServer);
			}
			if (bundle.cmd == Command.ServerSelect)
			{
				spin.SetActive (true);
				PanelsManager.Instance.Close ();
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
