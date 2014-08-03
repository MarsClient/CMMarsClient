using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Asset loader.
/// Is Game's Resource Handler...
/// </summary>

public class AssetLoader : MonoBehaviour {

	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	void Awake () {if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject); }

	private Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle> ();
	private Dictionary<string, AssetBundle> scAssetBundles = new Dictionary<string, AssetBundle> ();

	public delegate void DownloadUpdateCallBack (float progress, string scName );
	public delegate void DownloadFinishCallBack (List <object> gos);
	public DownloadUpdateCallBack updateCallBack;//progress.....Loading

	public void Start ()
	{
		scenePath = 
#if UNITY_ANDROID
		"file://" + Application.dataPath + "/A_MarsRes/Android/SC/";;
			//"file:///mnt/sdcard/MarsRes/SC/";;
#elif UNITY_IPHONE
	"file://" + Application.dataPath + "/A_MarsRes/IOS/SC/";;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/A_MarsRes/PC/SC/";
#endif
		if (Application.platform == RuntimePlatform.Android)
		{
			scenePath = "file:///mnt/sdcard/MarsRes/SC/";;
		}

		assetBundlePath = 
#if UNITY_ANDROID
		"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";;
			//"file:///mnt/sdcard/MarsRes/{0}/{1}";
#elif UNITY_IPHONE
		"file://" + Application.dataPath + "/A_MarsRes/IOS/{0}/{1}";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/A_MarsRes/PC/{0}/{1}";
#endif
		if (Application.platform == RuntimePlatform.Android)
		{
			assetBundlePath = "file:///mnt/sdcard/MarsRes/{0}/{1}";
		}

	}

	public void DownloadScenes (string fileName)
	{
		StartCoroutine (_DownloadScenes (fileName));
	}

	public void DownloadAssetbundle (string[] fileName, DownloadFinishCallBack callback, bool isDontDestory = false)
	{
		StartCoroutine (DownloadAssetBundle (fileName, callback, isDontDestory));
	}

	IEnumerator _DownloadScenes (string sc)
	{ 
		m_Progress = 0;
		string path = scenePath + sc + ".unity3d";
		if (scAssetBundles.ContainsKey (sc) == false || scAssetBundles[sc] == null)
		{
			WWW www = new WWW(path);  
			yield return www;  
			if (www.error == null)
			{
				AssetBundle bundle = www.assetBundle;  
				scAssetBundles[sc] = bundle;
			}
			else
			{
				Debug.LogError (www.error + "  and" + sc);
			}
		}
		UpdateCallBack (0.2f, sc);
	}
	private float m_Progress = 0;
	IEnumerator DownloadAssetBundle (string[] scs, DownloadFinishCallBack callback, bool isDontDestory)
	{
		List<object> gos = new List<object> ();

		foreach (string sc in scs)
		{
			m_Progress++;
			if (assetBundles.ContainsKey (sc) == false || assetBundles[sc] == null)
			{
				string[] files = sc.Split('/');
				string path = string.Format (assetBundlePath, files[0], files[1] + ".assetbundle");
				//Debug.LogError (path);
				WWW www = new WWW (path);
				yield return www;
				if (www.error == null)
				{
					AssetBundle assetBundle = www.assetBundle;
					GameObject go = (GameObject) assetBundle.mainAsset;
					gos.Add (go);
					assetBundles[sc] = assetBundle;
					www.Dispose ();
					www = null;
					if (isDontDestory)
					{
						assetBundle.Unload (false);
					}
				}
			}
			else
			{
				gos.Add (assetBundles[sc].mainAsset);
			}
			UpdateCallBack (0.1f);
		}
		Callback (callback, gos);
	}

	void Callback (DownloadFinishCallBack callback, List <object> gos)
	{
		if (callback != null)
		{
			callback (gos);
		}
	}

	void UpdateCallBack (float progress, string sc = null)
	{
		if (updateCallBack != null)
		{
			updateCallBack (progress, sc);
		}
	}

	public void OnDisable ()
	{
		foreach (KeyValuePair<string, AssetBundle> kvp in scAssetBundles)
		{
			kvp.Value.Unload (false);
		}
		scAssetBundles.Clear ();
		Resources.UnloadUnusedAssets();
		GC.Collect ();
	}
}
