using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DC = DebugConsole;

public class NetTest : MonoBehaviour {

	private const string repose = "<<<<<<<repose<<<<<<<";

	void Start ()
	{
		DC.RegisterCommand (Command.CreatTeam.ToString (), AllTeamsEvent);
		DC.RegisterCommand (Command.JoinTeam.ToString (), AllTeamsEvent);
		DC.RegisterCommand (Command.LeftTeam.ToString (), AllTeamsEvent);
		DC.RegisterCommand (Command.SwapTeamLeader.ToString (), AllTeamsEvent);
		DC.RegisterCommand (Command.DismissTeam.ToString (), AllTeamsEvent);
	}

	private string AllTeamsEvent (params string[] p)
	{
		if (p.Length == 2)
		{
			Role role = new Role ();
			try 
			{
				role.roleId = long.Parse (p[1]);
				if (p[0] == Command.JoinTeam.ToString ().ToLower()) NetSend.SendJoinTeam(role);
				else if (p[0] == Command.LeftTeam.ToString ().ToLower()) NetSend.SendLeftTeam(role);
				else if (p[0] == Command.SwapTeamLeader.ToString ().ToLower()) NetSend.SendSwapTeamLeader(role);
				return repose;
			}
			catch (System.Exception e) {}
			finally {};
		}
		else if (p.Length == 1)
		{
			if (p[0] == Command.CreatTeam.ToString ().ToLower()) NetSend.SendCreatTeam();
			else if (p[0] == Command.DismissTeam.ToString ().ToLower()) NetSend.SendDismissTeam();
			return repose;
		}
		return p[0] + " roleId usage";
	}
}
