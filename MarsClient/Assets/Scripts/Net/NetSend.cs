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

	public static void SendJoinTeam (Team team)
	{
		PhotonClient.SendServer (Command.JoinTeam, team);
	}

	public static void SendLeaveTeam (Team team)
	{
		PhotonClient.SendServer (Command.LeaveTeam, team);
	}

	public static void SendSwapTeamLeader (Team team)
	{
		PhotonClient.SendServer (Command.SwapTeamLeader, team);
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

	public static void SendMonsterRefresh (FightRegion region)
	{
		PhotonClient.SendServer (Command.MonsterRefresh, region);
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
