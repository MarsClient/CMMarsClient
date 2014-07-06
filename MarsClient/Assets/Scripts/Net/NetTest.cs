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
		if (p.Length >= 2)
		{
			Role role = new Role ();
			try 
			{
				role.roleId = long.Parse (p[1]);
				if (p[0] == Command.CreatRole.ToString ()) NetSend.SendCreatTeam(role);
				else if (p[0] == Command.JoinTeam.ToString ()) NetSend.SendJoinTeam(role);
				else if (p[0] == Command.LeftTeam.ToString ()) NetSend.SendLeftTeam(role);
				else if (p[0] == Command.SwapTeamLeader.ToString ()) NetSend.SendSwapTeamLeader(role);
				else if (p[0] == Command.DismissTeam.ToString ()) NetSend.SendDismissTeam(role);
				return repose;
			}
			catch (System.Exception e) {}
			finally {};
		}
		return p[0] + " roleId usage";
	}
}
