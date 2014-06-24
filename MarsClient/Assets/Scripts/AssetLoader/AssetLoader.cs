using System; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {

	private string scenePath = "";
	private string assetBundlePath = "";

	public static AssetLoader Instance;

	public Dictionary<string, GameObject> assetBundles = new Dictionary<string, GameObject> ();


	public void Awake ()
	{
		Instance = this;
		scenePath = 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/A_MarsRes/Android/SC/";
#elif UNITY_ANDROID
	"";
#elif UNITY_IPHONE
	"";
#endif

		assetBundlePath = 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			"file://" + Application.dataPath + "/A_MarsRes/Android/{0}/{1}";
#elif UNITY_ANDROID
"";
#elif UNITY_IPHONE
"";
#endif

	}

	public GameObject TryGameObjectByAssetBundles (string fileName)
	{
		GameObject go = null;
		assetBundles.TryGetValue (fileName, out go);
		if (go == null)
		{
			Download (fileName);
		}
		return go;
	}

	public void Download (string fileName, bool isScene = false)
	{
		if (isScene == true)
		{
			StartCoroutine (DownloadScenes (fileName));
		}
		else
		{
			StartCoroutine (DownloadAssetBundle (fileName));
		}
	}

	IEnumerator DownloadScenes (string sc)
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

	IEnumerator DownloadAssetBundle (string sc)
	{
		string path = string.Format (assetBundlePath, sc.Substring (0, 2), sc + ".assetbundle");
		Debug.LogError (path);
		WWW www = new WWW (path);
		yield return www;
		if (www.error == null)
		{
			GameObject go = (GameObject) www.assetBundle.mainAsset;
			if (assetBundles.ContainsKey (sc) == false)
			{
				assetBundles.Add (sc, go);
			}
			else
			{
				assetBundles[sc] = go;
			}
			www.Dispose ();
			www = null;
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
