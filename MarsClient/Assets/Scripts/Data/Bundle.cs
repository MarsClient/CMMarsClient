using UnityEngine;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

public class Bundle
{
	[DefaultValue (false)]
	public bool isRegister;
	[DefaultValue (null)]
	public string error;
	[DefaultValue (null)]
	public string notice;
	[DefaultValue (null)]
	public Command cmd;
	[DefaultValue (null)]
	public EventCommand eventCmd;

	[DefaultValue (null)]
	public Fight figth;

	[DefaultValue (null)]
	public Account account;
	[DefaultValue (null)]
	public User user;
	[DefaultValue (null)]
	public Room room;
	[DefaultValue (null)]
	public List<Room> rooms;
	[DefaultValue (null)]
	public Message mesaage;
	[DefaultValue (null)]
	public RoomMember roomMember;
}