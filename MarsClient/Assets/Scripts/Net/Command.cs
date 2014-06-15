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


	//Follow is old
	GetRoomInfo,
	GetAllRoomInfo,
	JoinRoom,
	QuitRoom,
	RoomActorBorning,
	RoomActorActionUpdate,
	RoomSpeak,
	UpdatePlayer,
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