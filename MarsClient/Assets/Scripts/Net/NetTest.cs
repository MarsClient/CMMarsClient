using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DC = DebugConsole;

public class NetTest : MonoBehaviour {

	private const string REPOSE = "<<<<<<<repose<<<<<<<";

	void Start ()
	{
		DC.RegisterCommand (Command.Login.ToString (), Login);
		DC.RegisterCommand (Command.JoinTeam.ToString (), JoinTeam);
		DC.RegisterCommand (Command.LeaveTeam.ToString (), LeaveTeam);
	}


	private string Login (params string[] p)
	{
		if (p.Length >= 3)
		{
			Account a = new Account();
			a.id = p[1];
			a.pw = p[2];
			NetSend.SendLogin (a);
			return REPOSE;
		}
		return "Login id, pw usage";
	}

	private string JoinTeam (params string[] p)
	{
		if (p.Length >= 3)
		{
			Team team = new Team ();
			team.teamName = p[1];
			team.teamId = p[2];
			NetSend.SendJoinTeam (team);
			return REPOSE;
		}
		return p[0] + " teamName, teamId usage";
	}

	private string LeaveTeam (params string[] p)
	{
		if (p.Length >= 2)
		{
			Team team = new Team ();
			team.teamId = p[1];
			NetSend.SendLeaveTeam (team);//
			return REPOSE;
		}
		return p[0] + " teamId usage";
	}
}
