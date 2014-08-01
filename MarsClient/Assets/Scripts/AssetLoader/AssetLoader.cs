using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {

	public UIFont normalFont;



	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	void Awake () {if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject); }

	public Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle> ();
	public Dictionary<string, AssetBundle> scAssetBundles = new Dictionary<string, AssetBundle> ();
	public delegate void DownloadFinishCallBack (List <object> gos);

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

//	public GameObject TryGameObjectByAssetBundles (string fileName)
//	{
//		GameObject go = null;
//		assetBundles.TryGetValue (fileName, out go);
//		if (go == null)
//		{
//			DownloadScenes (fileName);
//		}
//		return go;
//	}

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
		string path = scenePath + sc + ".unity3d";
		if (scAssetBundles.ContainsKey (sc) == false || scAssetBundles[sc] == null)
		{
//			Debug.LogError (path);
			WWW www = new WWW(path);  
			yield return www;  
			if (www.error == null)
			{
				AssetBundle bundle = www.assetBundle;  
				//Application.LoadLevel (sc);
				scAssetBundles[sc] = bundle;
				//bundle.Unload (false);
				//scAssetBundles.Add (sc, bundle);
			}
			else
			{
				Debug.LogError (www.error + "  and" + sc);
			}
		}
		Debug.Log (ScenesManager.instance);
		if (ScenesManager.instance != null)
		{
			//Application.LoadLevelAdditive (sc);
			Application.LoadLevel (sc);
			//StartCoroutine (UISceneLoading.instance.LoadAssetBundleScenes ( Application.LoadLevelAdditiveAsync (sc)));  
		}
		else
		{
			Application.LoadLevelAdditiveAsync (sc);
		}
	}

	IEnumerator DownloadAssetBundle (string[] scs, DownloadFinishCallBack callback, bool isDontDestory)
	{
		List<object> gos = new List<object> ();
		foreach (string sc in scs)
		{
			if (assetBundles.ContainsKey (sc) == false || assetBundles[sc] == null)
			{
				string[] files = sc.Split('/');
				string path = string.Format (assetBundlePath, files[0], files[1] + ".assetbundle");
				//Debug.LogError (path);
				WWW www = new WWW (path);
				yield return www;
				if (www.error == null)
				{
//					Debug.Log (sc);
					AssetBundle assetBundle = www.assetBundle;
					GameObject go = (GameObject) assetBundle.mainAsset;
	//				if (assetBundles.ContainsKey (sc) == false)
	//				{
	//					assetBundles.Add (sc, go);
	//				}
	//				else
	//				{
	//					assetBundles[sc] = go;
	//				}
					//Debug.LogError (assetBundles.Count);
					gos.Add (go);
					assetBundles[sc] = assetBundle;
					www.Dispose ();
					www = null;
					if (isDontDestory)
					{
						assetBundle.Unload (false);
					}
					//assetBundles.Add (sc, go);
				}
			}
			else
			{
				gos.Add (assetBundles[sc].mainAsset);
			}
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

//	void OnGUI ()
//	{
//		//Debug.LogError (parent.transform.childCount);
//		if (GUILayout.Button ("hahahahhahahah"))
//		{
//			Download ("PublicZone.unity3d");
//		}
//	}
}
