using UnityEngine;
using System.Collections;

public class UISceneLoading : MonoBehaviour {


	public const string SPLASH = "Splash";
	public const string PUBLIC_ZONE = "PublicZone";

	public delegate void OnSenceLoadingDone (string str);

	public static OnSenceLoadingDone currentOnSenceLoadingDone;
	public static UISceneLoading instance;
	public static string currentLoadName = "Splash";

	public UISlider slider;

	//public static void Load

	void Awake () { instance = this; }
	void OnDisable () { instance = null; }


	public static void LoadingScnens (string loadName)
	{
		LoadingScnens (loadName, null);
	}
	public static void LoadingScnens (string loadName, OnSenceLoadingDone onSenceLoadingDone)
	{
		currentOnSenceLoadingDone = onSenceLoadingDone;
		currentLoadName = loadName;
		Application.LoadLevel ("Loading");
	}


	private AsyncOperation async;
	IEnumerator  Start ()
	{
		if (currentLoadName != null)
		{
			async = Application.LoadLevelAdditiveAsync (currentLoadName);
			//loadName = null;
			yield return async;
			yield return new WaitForSeconds (0.5f);
			if (currentOnSenceLoadingDone != null)
			{
				currentOnSenceLoadingDone (currentLoadName);
			}
			//currentLoadName = null;
			currentOnSenceLoadingDone = null;
			Destroy (gameObject);
		}
	}

	void Update ()
	{
		if (async != null)
		{
			if (async.isDone == false)
			{
				slider.value = async.progress;
			}
			else
			{
				slider.value = 1.0f;
			}
		}
	}

//	void OnGUI ()
//	{
//		if (GUILayout.Button ("Spell1"))
//		{
//			LoadingScnens ("Splash");
//		}
//		if (GUILayout.Button ("Spell2"))
//		{
//
//		}
//	}
}
