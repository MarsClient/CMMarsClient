using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ToolEditor : Editor {

	[MenuItem("Game Editor/Create Json")]  
	public static void CreatJsonText ()
	{
		string json ="";

		/*Bundle bundle = new Bundle ();
		bundle.error = new Error ();
		bundle.error.message = "game.server.net.error";
		bundle.cmd = Command.NetError;
		json = JsonConvert.SerializeObject (bundle);
		json = NetEncrypt.Encrypt (json);*/

		Debug.Log (json);
	}
}
