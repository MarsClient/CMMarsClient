using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class AppServerManager : MonoBehaviour {

	public static AppServerManager Instance;

	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}

	
#if UNITY_ANDROID

	public class Func
	{
		[DefaultValue(null)]
		public string func;
		[DefaultValue(null)]
		public string val;
	}

	void CallAndroidCommand (params string[] paraterm)
	{
		Func func = new Func();
		func.func = paraterm[0];
		if (paraterm.Length == 2)
		{
			func.val = paraterm[1];
		}
		string value = JsonConvert.SerializeObject (func);
		Debug.Log (value);
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call ("CallAndroidActivity", value);
	}
#endif


	/*string mesage = "";
	void Test (string val)
	{
		this.mesage = val;
	}
	void OnGUI ()
	{
		if (GUILayout.Button ("call android"))
		{
			CallAndroidCommand ("Test", "hahha");
		}

		if (GUILayout.Button ("call android"))
		{
			CallAndroidCommand ("Test111111");
		}

		GUILayout.Label ("_____" + mesage);
	}*/

}
