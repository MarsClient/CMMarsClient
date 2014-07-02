using UnityEngine;
using System.Collections;

public class UISceneLoading : MonoBehaviour {


	public const string SPLASH = "Splash";
	public const string PUBLIC_ZONE = "PublicZone";

	public delegate void OnSenceLoadingDone (string str);

	public static OnSenceLoadingDone currentOnSenceLoadingDone;
	public static UISceneLoading instance;
	public static string currentLoadName = "Splash";
	public static bool isAssetBundle = false;

	public UISlider slider;

	//public static void Load

	void OnEnable () { instance = this; slider.value = 0; }
	//void OnDisable () { instance = null; }

	public static void LoadingScnens (string loadName)
	{
		LoadingScnens (loadName, null, false);
	}

	public static void LoadingScnens (string loadName, bool _isAssetBundle)
	{
		LoadingScnens (loadName, null, _isAssetBundle);
	}
	public static void LoadingScnens (string loadName, OnSenceLoadingDone onSenceLoadingDone, bool _isAssetBundle)
	{
		AssetLoader.Instance.OnDisable ();
		isAssetBundle = _isAssetBundle;
		currentOnSenceLoadingDone = onSenceLoadingDone;
		currentLoadName = loadName;
		Application.LoadLevel ("Loading");
		if (_isAssetBundle == true)
		{
			AssetLoader.Instance.DownloadScenes (loadName);
		}

		//
	}


	private AsyncOperation async;
	IEnumerator  Start ()
	{
		if (currentLoadName != null && isAssetBundle == false)
		{
			isAssetBundle = false;
			async = Application.LoadLevelAdditiveAsync (currentLoadName);
			//loadName = null;
			yield return async;
			if (GameData.Instance.isLoadingSuccess)
			{
				DelaySuccessLoading ();
			}
		}
	}

	public IEnumerator LoadAssetBundleScenes (AsyncOperation async)
	{
		this.async  = async;
		yield return async;
		//DelaySuccessLoading ();

	}

	public void DelaySuccessLoading ()
	{
		//yield return new WaitForSeconds (0.5f);
		if (currentOnSenceLoadingDone != null)
		{
			currentOnSenceLoadingDone (currentLoadName);
		}
		slider.value = 1;
		//currentLoadName = null;
		currentOnSenceLoadingDone = null;
		TweenAlpha.Begin (gameObject, 2f, 0);
		Destroy (gameObject, 2f);

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
