using UnityEngine;
using System.Collections;

public enum Command : byte
{
	Handshake,
	Login,
	Register,
	ServerList,

	LinkServer,
	ServerSelect,

	CreatRole,

	EnterGame,

	SendChat,

	InitAllPlayers,
	AddNewPlayer,
	UpdatePlayer,
	DestroyPlayer,

	CreatTeam,
	JoinTeam,
	LeftTeam,
	SwapTeamLeader,
	DismissTeam,

	EnterFight,

	PlayerDone,

	//Last
	NetError,
	AbortDiscount,
}

public enum EventCommand: byte
{
	LobbyBroadcast = 1,
	RoomBroadcastActorAction,
	RoomBroadcastActorQuit,
	RoomBroadcastActorSpeak,
	JoinRoomNotify,
	InitAllPlayer,
	UpdatePlayer,
	PlayerDisConnect,
}