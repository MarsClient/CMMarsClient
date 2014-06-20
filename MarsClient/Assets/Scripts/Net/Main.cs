using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour 
{
	public static Main Instance;

	public SQLiteVer sqliteVer;
	public Dictionary<string, Server[]> serverList;
	public Account account;
	public Server server;
	public List<Role> roles;
	public Role role;

	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}

//	void OnGUI ()
//	{
//		if (GUILayout.Button ("haha"))
//		{
//			Application.LoadLevel ("c");
//		}
//	}
}
