using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Asset loader.
/// Is Game's Resource Handler...
/// </summary>

public class AssetLoader : MonoBehaviour {

	private static object obj = new object();

	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	void Awake () {if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject); }

	private Dictionary<string, object> assetBundles = new Dictionary<string, object> ();
	private Dictionary<string, AssetBundle> scAssetBundles = new Dictionary<string, AssetBundle> ();

	public delegate void DownloadUpdateCallBack (float progress, string scName );
	public delegate void DownloadFinishCallBack (List <object> gos);
	public DownloadUpdateCallBack updateCallBack;//progress.....Loading

	public void Start ()
	{
		scenePath = 
#if UNITY_ANDROID
		"file://" + Application.dataPath + "/A_MarsRes/Android/ + Constants.SC";
#elif UNITY_IPHONE
		"file://" + Application.dataPath + "/A_MarsRes/IOS/" + Constants.SC;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		"file://" + Application.dataPath + "/A_MarsRes/PC/" + Constants.SC;
#endif
		if (Application.platform == RuntimePlatform.Android)
		{
			scenePath = "file:///mnt/sdcard/MarsRes/" + Constants.SC;
		}

		assetBundlePath = 
#if UNITY_ANDROID
		"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";;
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
		StartCoroutine (CoroutineDownloadScenes (fileName));
	}

	public void DownloadAssetbundle (string[] fileName, DownloadFinishCallBack callback, bool isDontDestory = false)
	{
		StartCoroutine (DownloadAssetBundle (fileName, callback, isDontDestory));
	}

	IEnumerator CoroutineDownloadScenes (string sc)
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

	public GameObject TryGetObj (string path)
	{
		if (assetBundles.ContainsKey (path))
		{
			GameObject g_obj = (GameObject) assetBundles [path];
			return g_obj;
		}
		return null;
	}

	private float m_Progress = 0;
	IEnumerator DownloadAssetBundle (string[] scs, DownloadFinishCallBack callback, bool isDontDestory)
	{
		lock (obj)
		{
			List<object> gos = new List<object> ();

			foreach (string sc in scs)
			{
				m_Progress++;
				if (assetBundles.ContainsKey (sc) == false || assetBundles[sc] == null)
				{
					string[] files = sc.Split('/');
					string path = string.Format (assetBundlePath, files[0], files[1] + ".assetbundle");
					WWW www = new WWW (path);
					yield return www;
					if (www.error == null)
					{
						AssetBundle assetBundle = www.assetBundle;
						object go = assetBundle.mainAsset;
						gos.Add (go);
						assetBundles[sc] = go;
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
					gos.Add (assetBundles[sc]);
				}

				if (ScenesManager.instance != null)
				{
					float progress = m_Progress / (float)scs.Length * 0.8f;
					UpdateCallBack (progress);
				}
			}
			Callback (callback, gos);
		}
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


	private Dictionary<string, GameObject> dontDestroyObjs = new Dictionary<string, GameObject> ();
	public delegate void LoadingDontDestroyFinishNotice (string[] pros);
	private LoadingDontDestroyFinishNotice m_loadingDontDestroyFinishNotice;
	private bool isInit = true;
	public void LoadingGameObjectWithDontDestroy (LoadingDontDestroyFinishNotice loadingDontDestroyFinishNotice)
	{
		if (isInit == false) return;
		isInit = false;

		this.m_loadingDontDestroyFinishNotice = loadingDontDestroyFinishNotice;
		List<string> keys = new List<string> ();
		keys.Add (Constants.PRO + Constants.RO_STRING + ((int)PRO.ZS).ToString ());
		keys.Add (Constants.PRO + Constants.RO_STRING + ((int)PRO.FS).ToString ());
		keys.Add (Constants.PRO + Constants.RO_STRING + ((int)PRO.DZ).ToString ());

		//TODO:

		if (keys.Count > 0)
		{
			AssetLoader.Instance.DownloadAssetbundle (keys.ToArray(), LoadingDontDestroyFinish, true);
		}
	}

	private void LoadingDontDestroyFinish (List<object> gos)
	{
		foreach (object o in gos)
		{
			GameObject go = (GameObject) o;
			string key = go.name;
			dontDestroyObjs.Add (key, go);
		}
		if (m_loadingDontDestroyFinishNotice != null)
		{
			m_loadingDontDestroyFinishNotice (pros);
		}
	}

	public GameObject TryGetDontDestroyObject (string key)
	{
		GameObject m_Go = null;
		dontDestroyObjs.TryGetValue (key, out m_Go);
		return m_Go;
	}

	#region PROS
	private Dictionary <PRO, GameObject> PROS = new Dictionary<PRO, GameObject>();
	private string[] m_pros;
	public string[] pros
	{
		get
		{
			m_pros = new string[3];
			m_pros[0] = Constants.RO_STRING + ((int)PRO.ZS).ToString ();
			m_pros[1] = Constants.RO_STRING + ((int)PRO.FS).ToString ();
			m_pros[2] = Constants.RO_STRING + ((int)PRO.DZ).ToString ();
			return m_pros;
		}
	}
	#endregion
}
