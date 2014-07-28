using UnityEngine;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

public class Bundle
{
	[DefaultValue (null)]
	public Command cmd;

	[DefaultValue (null)]
	public SQLiteVer sqliteVer;

	[DefaultValue (null)]
	public Dictionary<string, Server[]> serverList;

	[DefaultValue (null)]
	public Error error;

	[DefaultValue (null)]
	public Account account;

	[DefaultValue(null)]
	 public Server server;

	[DefaultValue(null)]
	public List<Role> roles;

	[DefaultValue(null)]
	public Role role;

	[DefaultValue(null)]
	public List<Role> onlineRoles;

	[DefaultValue(null)]
	public Message message;

	[DefaultValue (null)]
	public Fight fight;

	[DefaultValue(null)]
	public string info;

	[DefaultValue(null)]
	public Team team;

	/*follow is old*/

//	[DefaultValue (null)]
//	public Fight figth;
//
//	[DefaultValue (null)]
//	public List<Player> players;
//
//	[DefaultValue(null)]
//	public Player player;

//	[DefaultValue (null)]
//	public User user;
//	[DefaultValue (null)]
//	public RoomInfo room;
//	[DefaultValue (null)]
//	public List<RoomInfo> rooms;
//	[DefaultValue (null)]
//	public Message mesaage;
//	[DefaultValue (null)]
//	public RoomMember roomMember;
}