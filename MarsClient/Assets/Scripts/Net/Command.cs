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

	EnterFight,

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