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
				Main.Instance.onlineRoles = bundle.onlineRoles;
				UISceneLoading.LoadingScnens (UISceneLoading.PUBLIC_ZONE, null, true);
			}

			if (bundle.cmd == Command.InitAllPlayers)
			{
				Main.Instance.onlineRoles = bundle.onlineRoles;
			}
			if (bundle.cmd == Command.UpdatePlayer)
			{

			}
			if (bundle.cmd == Command.DestroyPlayer)
			{
				
			}
			if (bundle.team != null)
			{
				Main.Instance.team = bundle.team;
			}
			if (bundle.cmd == Command.EnterFight)
			{
				Main.Instance.fight = bundle.fight;
				Main.Instance.team = bundle.team;
				UISceneLoading.LoadingScnens ("LV00001A", null, true);
			}
		}
		else
		{
			if (bundle.cmd == Command.NetError)
			{
				new DialogContent ()
					.SetMessage(bundle.error.message)
						.SetNoBtn ("game.dialog.yes")
						.SetDelegateBtn ((bool isBy)=>
						{
							//if (UISceneLoading.currentLoadName != UISceneLoading.SPLASH)
							//{
							UISceneLoading.LoadingScnens (UISceneLoading.SPLASH, (string loadName)=>
							{
								Debug.Log (loadName);
								NetSend.SendAbortDiscount ();
								PhotonClient.Instance.LoadingLoginServer ();

								PanelsManager.Close ();
								PanelsManager.Show (PanelType.ServerList, (Panel panel)=>
								                    {
									UITabServerList tabServerTabList = panel.GetComponentInChildren<UITabServerList>();
									if (tabServerTabList != null) tabServerTabList.Initialization ();
								});

							}, false);
							//}
//							PanelsManager.Close ();
//							PanelsManager.Show (PanelType.ServerList, (Panel panel)=>
//							{
//								UITabServerList tabServerTabList = panel.GetComponentInChildren<UITabServerList>();
//								if (tabServerTabList != null) tabServerTabList.Initialization ();
//							});
						})
						.ShowWaiting ();

			}
		}
	}

	public void ProcessResultSync (Bundle bundle)
	{
		if (bundle.error == null)
		{
			if (bundle.cmd == Command.SendChat)
			{
				Message message = bundle.message;
				if (message != null)
				{
					if (Main.Instance.messages.ContainsKey (message.chatType) == false)
					{
						Main.Instance.messages.Add (message.chatType, new List<Message>());
					}
					else
					{
						Main.Instance.messages[message.chatType].Add (message);
					}
				}
			}
			if (bundle.cmd == Command.EnterFight)
			{
				Main.Instance.fight = bundle.fight;
				new DialogContent()
					.SetMessage("game.fight.messgae", "ICC")
						.SetYesBtn ("game.dialog.yes")
						.SetNoBtn ("game.dialog.no")
						.SetDelegateBtn ((bool isBy)=>
						{
							if (isBy)
							{
								Fight fight = new Fight ();
								fight.id = bundle.fight.id;
								NetSend.SendEnterFight (fight);
							}
						})
						.Show ();
			}
		}
	}

	void OnGUI ()
	{
		if (GUILayout.Button ("test"))
		{
			Bundle bundle = new Bundle ();
			bundle.error = new Error ();
			bundle.error.message = "game.server.net.error";
			bundle.cmd = Command.NetError;
			ProcessResult (bundle);
		}
	}
}
