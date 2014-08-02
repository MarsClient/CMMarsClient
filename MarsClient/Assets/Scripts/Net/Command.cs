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
	LeaveTeam,
	SwapTeamLeader,
	DismissTeam,
	TeamUpdate,
	PlayerAdd,

	EnterFight,

	PlayerDone,

	MonsterStateUpdate,

	//Last
	NetError,
	AbortDiscount,
}