﻿using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {

	public UIFont normalFont;



	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	void Awake () {if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject); }

	public Dictionary<string, GameObject> assetBundles = new Dictionary<string, GameObject> ();
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
	"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";
#endif
		if (Application.platform == RuntimePlatform.Android)
		{
			assetBundlePath = "file:///mnt/sdcard/MarsRes/{0}/{1}";
		}

	}

	public GameObject TryGameObjectByAssetBundles (string fileName)
	{
		GameObject go = null;
		assetBundles.TryGetValue (fileName, out go);
		if (go == null)
		{
			DownloadScenes (fileName);
		}
		return go;
	}

	public void DownloadScenes (string fileName)
	{
		StartCoroutine (_DownloadScenes (fileName));
	}

	public void DownloadAssetbundle (string[] fileName, DownloadFinishCallBack callback)
	{
		StartCoroutine (DownloadAssetBundle (fileName, callback));
	}

	IEnumerator _DownloadScenes (string sc)
	{ 
		string path = scenePath + sc + ".unity3d";
		if (scAssetBundles.ContainsKey (sc) == false || scAssetBundles[sc] == null)
		{
			//Debug.LogError (path);
			WWW www = new WWW(path);  
			yield return www;  
			if (www.error == null)
			{
				AssetBundle bundle = www.assetBundle;  
				//Application.LoadLevel (sc);
				scAssetBundles.Add (sc, bundle);
			}
			else
			{
				Debug.LogError (www.error + "  and" + sc);
			}
		}
		Debug.Log (UISceneLoading.instance);
		if (UISceneLoading.instance != null)
			StartCoroutine (UISceneLoading.instance.LoadAssetBundleScenes ( Application.LoadLevelAdditiveAsync (sc)));  
		else
		{
			Application.LoadLevelAdditiveAsync (sc);
		}
	}

	IEnumerator DownloadAssetBundle (string[] scs, DownloadFinishCallBack callback)
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
					Debug.Log (sc);
					GameObject go = (GameObject) www.assetBundle.mainAsset;
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
					www.Dispose ();
					www = null;
					assetBundles.Add (sc, go);
				}
			}
			else
			{
				gos.Add (assetBundles[sc]);
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
		Resources.UnloadUnusedAssets();
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
