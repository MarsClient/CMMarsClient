using UnityEngine;
using System.Collections;

public class ScenesManager : MonoBehaviour {


	public const string SPLASH = "Splash";
	public const string PUBLIC_ZONE = "PublicZone";

	public delegate void OnSenceLoadingDone (string str);

	public static OnSenceLoadingDone currentOnSenceLoadingDone;
	public static ScenesManager instance;
	public static string currentLoadName = "Splash";
	public static bool isAssetBundle = false;

	public SliderTween slider;

	//public static void Load

	void Awake () { instance = this; DontDestroyOnLoad (gameObject); AssetLoader.Instance.updateCallBack  = UpdateProgress; }

	void OnDestroy () { instance = null; AssetLoader.Instance.updateCallBack  = null; }

	void OnEnable () {  StartCoroutine (LoadingNewSc ()); slider.value = 0; }
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
	}

	IEnumerator  LoadingNewSc ()
	{
		if (currentLoadName != null && isAssetBundle == false)
		{
			isAssetBundle = false;
			AsyncOperation async = Application.LoadLevelAdditiveAsync (currentLoadName);
			//loadName = null;
			yield return async;
			if (GameData.Instance.isLoadingSuccess)
			{
				DelaySuccessLoading ();
			}
		}
	}

	public void DelaySuccessLoading ()
	{
		if (currentOnSenceLoadingDone != null)
		{
			currentOnSenceLoadingDone (currentLoadName);
		}
		slider.value = 1;
		TweenAlpha.Begin (slider.gameObject, 0.5f, 0);
		currentOnSenceLoadingDone = null;
		TweenAlpha.Begin (gameObject, 2f, 0);
		Destroy (gameObject, 2f);
	}

	void UpdateProgress (float progress, string scName)
	{
		float m_p = slider.value + progress;
		slider.value += Mathf.Min (1, m_p);

		if (scName != null)
		{
			Application.LoadLevel (scName);
		}

//		Debug.LogError (progress + "___" + slider.value);
	}

}
