using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	public static Main Instance;

	public SQLiteVer sqliteVer;
	public Account account;

	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}

	void OnGUI ()
	{
		if (GUILayout.Button ("haha"))
		{
			Application.LoadLevel ("c");
		}
	}
}
