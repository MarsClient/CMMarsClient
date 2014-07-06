using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Team 
{
	[DefaultValue (0L)]
	public long teamId;
	[DefaultValue (null)]
	public List<Role> roles;


	public bool isLead
	{
		get
		{
			if (Main.Instance.role != null && Main.Instance.role.roleId == teamId)
			{
				return true;
			}
			return false;
		}
	}
}