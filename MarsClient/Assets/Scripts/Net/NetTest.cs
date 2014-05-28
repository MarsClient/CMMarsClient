using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DC = DebugConsole;

public class NetTest : MonoBehaviour {

	private const string repose = "<<<<<<<repose<<<<<<<";

	public bool isOpenDenug = true;

	void Start () 
	{
		DC.IsOpen = isOpenDenug;
		DC.LogWarning ("A,S,D,W controll player dir;\n right mouse click attack \n button spell1,spell2,spell3 is use spell");
		DC.RegisterCommand ("LoadRes", InitRes);
		DC.RegisterCommand (Command.Register.ToString (), RegisterTest);
		DC.RegisterCommand (Command.Login.ToString (), LoginTest);
		DC.RegisterCommand (Command.GetRoomInfo.ToString (), GetRoomInfo);
		DC.RegisterCommand (Command.JoinRoom.ToString (), JoinRoom);
		DC.RegisterCommand (Command.GetAllRoomInfo.ToString (), GetAllRoomInfo);
		DC.RegisterCommand (Command.QuitRoom.ToString (), QuitRoom);
		DC.RegisterCommand (Command.RoomSpeak.ToString (), RoomSpeak);

		DC.RegisterCommand ("FightTest", InitTroops);
	}

	private string InitRes (params string[] p)
	{
		if (p.Length >= 1)
		{
			//AssetLoader.instance.Start ();
			return repose;
		}
		return p[0] + " usage";
	}

	private string RegisterTest (params string[] p)
	{
		if (p.Length >= 4)
		{
			Account a = new Account ();
			a.id = p[1];
			a.pw = p[2];
			a.roleName = p[3];
			NetSend.SendRegister (a);
			//NotificationCenter.PostNotification (this, "SendRegister", a);
			return repose;
		}
		return p[0] + " userId,userPw";
	}

	private string LoginTest (params string[] p)
	{
		if (p.Length >= 3)
		{
			Account user = new Account ();
			user.id = p[1];
			user.pw = p[2];
			NetSend.SendLogin (user);
			return repose;
		}
		return p[0] + " ID" + " PW usage";
	}

	private string GetRoomInfo (params string[] p)
	{
		if (p.Length >= 2)
		{
			RoomInfo room = new RoomInfo ();
			try
			{
				room.id = int.Parse (p[1]);
				NetSend.SendGetRoomInfo(room);
				return repose;
			}
			catch (System.Exception e)
			{

			}
		}
		return p[0] + " roomId usage";
	}

	private string JoinRoom (params string[] p)
	{
		if (p.Length >= 2)
		{
			RoomInfo room = new RoomInfo ();
			try
			{
				room.id = int.Parse (p[1]);
				NetSend.SendJoinRoom (room);
				return repose;
			}
			catch (System.Exception e)
			{
				
			}
		}
		return p[0] + " RoomId usage";
	}

	private string GetAllRoomInfo (params string[] p)
	{
		if (p.Length >= 1)
		{
			NetSend.SendGetAllRoomInfo ();
			//NotificationCenter.PostNotification (this, "SendGetAllRoomInfo");
			return repose;
		}
		return p[0] +" usage";
	}

	private string QuitRoom (params string[] p)
	{
		if (p.Length >= 1)
		{
			NetSend.SendQuitRoom ();
			//NotificationCenter.PostNotification (this, "SendQuitRoom");
			return repose;
		}
		return p[0] +" usage";
	}

	private string RoomSpeak (params string[] p)
	{
		if (p.Length >= 2)
		{
			Message message = new Message ();
			message.content = p[1];
			NetSend.SendRoomSpeak (message);
			//NotificationCenter.PostNotification (this, "SendRoomSpeak", message);
			return repose;
		}
		return p[0] + " message usage";
	}

	private string InitTroops (params string[] p)
	{
		if (p.Length >= 1)
		{
//			if (AssetLoader.instance.isSuccess)
//			{
//				FightTest();
//				return repose;
//			}
//			else
//			{
//				return "wait resource loading done";
//			}
			return repose;
		}
		return p[0] + " usage";
	}

	public static void FightTest ()
	{
		TextAsset textAsset = Resources.Load ("report", typeof (TextAsset)) as TextAsset;
		//DC.Log (textAsset.text);
		Bundle bundle = JsonConvert.DeserializeObject<Bundle>(textAsset.text);
		PhotonClient.Instance.CalledProcessResult(bundle);
//		Bundle bundle = new Bundle();
//		bundle.figth = new Fight ();
//		bundle.figth.players = new List<Troop> ();
//		bundle.figth.enemys = new List<Troop> ();
//		Troop troop = GameData.Instance.getPlayerById ("P0001");
//		bundle.figth.players.Add (troop);
//		for (int i = 0; i < 4; i++)
//		{
//			foreach (Troop t in GameData.Instance.getTroops ())
//			{
//				bundle.figth.enemys.Add (t);
//			}
//		}
//		bundle.figth.map = new Map ();
//		bundle.figth.map.mapName = "MP001";
//		bundle.figth.map.mapHeigh = 200;
//		bundle.figth.map.mapWidth = 2000;
//
//		string json = JsonConvert.SerializeObject (bundle);
//		Debug.Log (json);
//		PhotonClient.Instance.CalledProcessResult(bundle);


	}
}
