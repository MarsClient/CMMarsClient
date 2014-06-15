using UnityEngine;
using System.Collections;

public class ButtonServerSelect : MonoBehaviour {

	public UIServerList serverList;

	void OnClick ()
	{
		if (serverList.currentServer != null)
		{
			PhotonClient.Instance.SetGameServer (serverList.currentServer.ip);
		}
	}
}
