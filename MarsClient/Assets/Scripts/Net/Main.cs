using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (FpsCounter))]
public class Main : MonoBehaviour 
{
	public static Main Instance;
	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}


	public SQLiteVer sqliteVer;
	public Dictionary<string, Server[]> serverList = new Dictionary<string, Server[]> ();
	public Account account;
	public Server server;
	public List<Role> roles = new List<Role> ();
	public Role role;
	public Dictionary<ChatType, List<Message>> messages = new Dictionary<ChatType, List<Message>> ();
	public List<Role> onlineRoles = new List<Role> ();
	public Team team;

	public Fight fight;


	public List<Message> Getmessages ()
	{
		return Getmessages (ChatType.Null, true);
	}

	public List<Message> Getmessages (ChatType chatType, bool isAll)//true is get all messages, false is get chat type
	{
		List<Message> messages_ = new List<Message> ();
		if (isAll)
		{
			foreach (List<Message> _messages in messages.Values )
			{
				messages_.AddRange (_messages);
			}
		}
		else
		{
			messages.TryGetValue (chatType, out messages_);
		}
		return messages_;

	}

	/****At last******/
	public void Clear ()
	{
		messages.Clear ();
		roles.Clear ();
	}


}
