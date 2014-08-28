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
		NetClient.SendServer (Command.Register, a);
	}

	public static void SendLogin (Account a)
	{
		NetClient.SendServer (Command.Login, a);
	}

	public static void SendServerSelect (Server server)
	{
		NetClient.SendServer (Command.ServerSelect, server);
	}

	public static void SendCreatRole (Role r)
	{
		NetClient.SendServer (Command.CreatRole, r);
	}

	public static void SendEnterGame (Role r)
	{
		NetClient.SendServer (Command.EnterGame, r);
	}

	public static void SendChat (Message message)
	{
		NetClient.SendServer (Command.SendChat, message);
	}

//	public static void SendRole (Role r)
//	{
//		PhotonClient.SendServer (Command.AddNewPlayer, r);
//	}

	public static void SendUpdatePlayer (Role r)
	{
		NetClient.SendServer (Command.UpdatePlayer, r);
	}

	#region Team
	public static void SendCreatTeam ()
	{
		NetClient.SendServer (Command.CreatTeam);
	}

	public static void SendJoinTeam (Team team)
	{
		NetClient.SendServer (Command.JoinTeam, team);
	}

	public static void SendLeaveTeam (Team team)
	{
		NetClient.SendServer (Command.LeaveTeam, team);
	}

	public static void SendSwapTeamLeader (Team team)
	{
		NetClient.SendServer (Command.SwapTeamLeader, team);
	}

	public static void SendDismissTeam ()
	{
		NetClient.SendServer (Command.DismissTeam);
	}

	public static void SendTeamUpdate (Role r)
	{
		NetClient.SendServer (Command.TeamUpdate, r);
	}
	#endregion

	#region Fight
	public static void SendEnterFight (Fight fight)
	{
		NetClient.SendServer (Command.EnterFight, fight);
	}

	public static void SendPlayersDone ()
	{
		NetClient.SendServer (Command.PlayerDone);
	}

	public static void SendMonsterRefresh (FightRegion region)
	{
		NetClient.SendServer (Command.MonsterRefresh, region);
	}

	public static void SendMonsterUpdate (GameMonster gameMonster)
	{
		NetClient.SendServer (Command.MonsterStateUpdate, gameMonster);
	}
	#endregion

	public static void SendDisconnectServer ()
	{
		NetClient.Instance.PeerDiscount ();
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
