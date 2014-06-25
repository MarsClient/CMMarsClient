using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {

	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	public Dictionary<string, GameObject> assetBundles = new Dictionary<string, GameObject> ();
	public delegate void DownloadFinishCallBack (List <object> gos);

	public void Awake ()
	{
		Instance = this;
		scenePath = 
#if UNITY_ANDROID
	"file://" + Application.dataPath + "/A_MarsRes/Android/SC/";;
#elif UNITY_IPHONE
	"file://" + Application.dataPath + "/A_MarsRes/IOS/SC/";;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/A_MarsRes/PC/SC/";
#endif

		assetBundlePath = 
#if UNITY_ANDROID
		"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";;
#elif UNITY_IPHONE
		"file://" + Application.dataPath + "/A_MarsRes/IOS/{0}/{1}";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";
#endif

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
		//Debug.LogError (path);
		WWW www = new WWW(path);  
		yield return www;  
		if (www.error == null)
		{
			AssetBundle bundle = www.assetBundle;  
			//Application.LoadLevel (sc);
			StartCoroutine (UISceneLoading.instance.LoadAssetBundleScenes ( Application.LoadLevelAdditiveAsync (sc)));  
		}
		else
		{
			Debug.LogError (www.error + "  and" + sc);
		}
	}

	IEnumerator DownloadAssetBundle (string[] scs, DownloadFinishCallBack callback)
	{
		List<object> gos = new List<object> ();
		foreach (string sc in scs)
		{
			string path = string.Format (assetBundlePath, sc.Substring (0, 2), sc + ".assetbundle");
	//		Debug.LogError (path);
			WWW www = new WWW (path);
			yield return www;
			if (www.error == null)
			{
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

	void OnDisable ()
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
