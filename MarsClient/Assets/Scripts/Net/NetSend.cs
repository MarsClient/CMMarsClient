using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

//Sned to server, follow;
//auto receive function "AutoProcessResult" parameter is Bunle
//http receive function "ProcessResult" parameter is Bundle

public class NetSend {

	public static void SendRegister (Account a)
	{
		PhotonClient.SendServer (Command.Register, a);
	}

	public static void SendLogin (Account a)
	{
		PhotonClient.SendServer (Command.Login, a);
	}

	public static void SendServerSelect (Server server)
	{
		PhotonClient.SendServer (Command.ServerSelect, server);
	}

	public static void SendCreatRole (Role r)
	{
		PhotonClient.SendServer (Command.CreatRole, r);
	}

	public static void SendEnterGame (Role r)
	{
		PhotonClient.SendServer (Command.EnterGame, r);
	}

	public static void SendChat (Message message)
	{
		PhotonClient.SendServer (Command.SendChat, message);
	}

//	public static void SendRole (Role r)
//	{
//		PhotonClient.SendServer (Command.AddNewPlayer, r);
//	}

	public static void SendUpdatePlayer (Role r)
	{
		PhotonClient.SendServer (Command.UpdatePlayer, r);
	}

	#region Team
	public static void SendCreatTeam ()
	{
		PhotonClient.SendServer (Command.CreatTeam);
	}

	public static void SendJoinTeam (Role r)
	{
		PhotonClient.SendServer (Command.JoinTeam, r);
	}

	public static void SendLeftTeam (Role r)
	{
		PhotonClient.SendServer (Command.LeftTeam, r);
	}

	public static void SendSwapTeamLeader (Role r)
	{
		PhotonClient.SendServer (Command.SwapTeamLeader, r);
	}

	public static void SendDismissTeam ()
	{
		PhotonClient.SendServer (Command.DismissTeam);
	}

	public static void SendTeamUpdate (Role r)
	{
		PhotonClient.SendServer (Command.TeamUpdate, r);
	}
	#endregion

	#region Fight
	public static void SendEnterFight (Fight fight)
	{
		PhotonClient.SendServer (Command.EnterFight, fight);
	}

	public static void SendPlayersDone ()
	{
		PhotonClient.SendServer (Command.PlayerDone);
	}
	#endregion

	public static void SendDisconnectServer ()
	{
		PhotonClient.Instance.PeerDiscount ();
	}
	//Follow is Older
	/*
	 *
	 */
//	public static void SendGetRoomInfo (RoomInfo room)
//	{
//		PhotonClient.SendServer (Command.GetRoomInfo, room);
//	}
//
//	public static void SendJoinRoom (RoomInfo room)
//	{
//		PhotonClient.SendServer (Command.JoinRoom, room);
//	}
//
//	public static void SendGetAllRoomInfo ()
//	{
//		PhotonClient.SendServer (Command.GetAllRoomInfo);
//	}
//
//	public static void SendQuitRoom ()
//	{
//		PhotonClient.SendServer (Command.QuitRoom);
//	}
//
//	public static void SendRoomSpeak (Message message)
//	{
//		PhotonClient.SendServer (Command.RoomSpeak, message);
//	}
//
//	public static void SendUpdatePlayerPos (Player p)
//	{
//		PhotonClient.SendServer (Command.UpdatePlayer, p);
//	}
}
