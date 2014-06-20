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
