using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Team 
{
	[DefaultValue (null)]
	public string teamId;
	[DefaultValue (0L)]
	public long fightId;
	[DefaultValue(null)]
	public string teamName;
	[DefaultValue (null)]
	public List<Role> roles;
}