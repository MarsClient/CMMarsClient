using System; 
using UnityEngine;
using System.Collections;

public class AssetLoader : MonoBehaviour {

	string scenePath = "";

	public static AssetLoader Instance;

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

	}

	public void Download (string fileName, bool isScene = false)
	{
		if (isScene == true)
		{
			StartCoroutine (DownloadScenes (fileName));
		}
		else
		{

		}
	}

	IEnumerator DownloadScenes (string sc)
	{ 
		string path = scenePath + sc + ".unity3d";
		//Debug.LogError (path);
		WWW scene = new WWW(path);  
		yield return scene;  
		if (scene.error == null)
		{
			AssetBundle bundle = scene.assetBundle;  
			//Application.LoadLevel (sc);
			StartCoroutine (UISceneLoading.instance.LoadAssetBundleScenes ( Application.LoadLevelAdditiveAsync (sc)));  
		}
		else
		{
			Debug.LogError (scene.error + "  and" + sc);
		}
	}

	IEnumerator DownloadAssetBundle (string sc)
	{
		yield return null;  
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
