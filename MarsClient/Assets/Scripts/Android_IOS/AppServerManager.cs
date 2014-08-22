using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class AppServerManager : MonoBehaviour {

	public static AppServerManager Instance;

	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}

	
#if UNITY_ANDROID

	T DoCommand<T> (string funcStr, params string[] paraterm)
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		return jo.Call<T> (funcStr, paraterm);
	}

	void DoCommand (string funcStr, params string[] paraterm)
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call ("DoCommand", funcStr, paraterm);
	}
#endif


	string message = "";
	void Test (string val)
	{
		this.message = val;
	}
	void OnGUI ()
	{
		if (GUILayout.Button ("call android"))
		{
			DoCommand ("Test", "hahha", "heihei");
		}

		if (GUILayout.Button ("call android"))
		{
			DoCommand ("Test111111");
		}

		if (GUILayout.Button ("GetVersion"))
		{
			message += DoCommand<int> ("GetVersion").ToString ();
		}

		GUILayout.Label ("_____" + message);
	}

}
