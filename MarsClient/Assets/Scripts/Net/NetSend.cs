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
		PhotonClient.Instance.SendServer (Command.Register, a);
	}

	public static void SendLogin (Account a)
	{
		PhotonClient.Instance.SendServer (Command.Login, a);
	}

	public static void SendServerSelect (Server server)
	{
		PhotonClient.Instance.SendServer (Command.ServerSelect, server);
	}

	public static void SendCreatRole (Role r)
	{
		PhotonClient.Instance.SendServer (Command.CreatRole, r);
	}

	public static void SendEnterGame (Role r)
	{
		PhotonClient.Instance.SendServer (Command.EnterGame, r);
	}

	public static void SendChat (Message message)
	{
		PhotonClient.Instance.SendServer (Command.SendChat, message);
	}

//	public static void SendRole (Role r)
//	{
//		PhotonClient.Instance.SendServer (Command.AddNewPlayer, r);
//	}

	public static void SendUpdatePlayer (Role r)
	{
		PhotonClient.Instance.SendServer (Command.UpdatePlayer, r);
	}

	#region Team
	public static void SendCreatTeam (Role r)
	{
		PhotonClient.Instance.SendServer (Command.CreatTeam, r);
	}

	public static void SendJoinTeam (Role r)
	{
		PhotonClient.Instance.SendServer (Command.JoinTeam, r);
	}

	public static void SendLeftTeam (Role r)
	{
		PhotonClient.Instance.SendServer (Command.LeftTeam, r);
	}

	public static void SendSwapTeamLeader (Role r)
	{
		PhotonClient.Instance.SendServer (Command.SwapTeamLeader, r);
	}

	public static void SendDismissTeam (Role r)
	{
		PhotonClient.Instance.SendServer (Command.DismissTeam, r);
	}
	#endregion

	public static void SendEnterFight (Fight fight)
	{
		PhotonClient.Instance.SendServer (Command.EnterFight, fight);
	}


	public static void SendAbortDiscount ()
	{
		PhotonClient.Instance.SendServer (Command.AbortDiscount);
	}
	//Follow is Older
	/*
	 *
	 */
//	public static void SendGetRoomInfo (RoomInfo room)
//	{
//		PhotonClient.Instance.SendServer (Command.GetRoomInfo, room);
//	}
//
//	public static void SendJoinRoom (RoomInfo room)
//	{
//		PhotonClient.Instance.SendServer (Command.JoinRoom, room);
//	}
//
//	public static void SendGetAllRoomInfo ()
//	{
//		PhotonClient.Instance.SendServer (Command.GetAllRoomInfo);
//	}
//
//	public static void SendQuitRoom ()
//	{
//		PhotonClient.Instance.SendServer (Command.QuitRoom);
//	}
//
//	public static void SendRoomSpeak (Message message)
//	{
//		PhotonClient.Instance.SendServer (Command.RoomSpeak, message);
//	}
//
//	public static void SendUpdatePlayerPos (Player p)
//	{
//		PhotonClient.Instance.SendServer (Command.UpdatePlayer, p);
//	}
}
