using UnityEngine;
using System.Collections;

public enum Command : byte
{
	Handshake,
	Login,
	Register,
	ServerList,
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