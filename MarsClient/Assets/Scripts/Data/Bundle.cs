using UnityEngine;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

public class Bundle
{
	[DefaultValue (null)]
	public Command cmd;

	[DefaultValue (null)]
	public EventCommand eventCmd;

	[DefaultValue (null)]
	public SQLiteVer sqliteVer;

	[DefaultValue (null)]
	public Dictionary<string, Server[]> serverList;

	[DefaultValue (null)]
	public Error error;

	[DefaultValue (null)]
	public Account account;

	/*follow is old*/

	[DefaultValue (null)]
	public Fight figth;

	[DefaultValue (null)]
	public List<Player> players;

	[DefaultValue(null)]
	public Player player;

	[DefaultValue (null)]
	public User user;
	[DefaultValue (null)]
	public RoomInfo room;
	[DefaultValue (null)]
	public List<RoomInfo> rooms;
	[DefaultValue (null)]
	public Message mesaage;
	[DefaultValue (null)]
	public RoomMember roomMember;
}