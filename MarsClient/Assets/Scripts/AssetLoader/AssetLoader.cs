using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {


	public static readonly string PathURL = 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		Application.dataPath + "/MGRes/AD/";
#elif UNITY_ANDROID   //安卓  
		"/mnt/sdcard/MGRes/";
#elif UNITY_IPHONE  //iPhone  
	Application.dataPath + "/MGRes/IOS/";
#else  
	string.Empty;  
#endif 

	//private Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle> ();

	private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject> ();
	public static AssetLoader instance; void Awake () { instance = this; }

	public delegate void LoadAssetComplete ();
	public LoadAssetComplete loadAssetComplete;

	[HideInInspector]
	public bool isSuccess = false;

	public void Start ()
	{
		StartCoroutine (LoadAssetRes ());
		StartCoroutine (GameData.Instance.reload ());
	}

	List<string> GetPathName ()
	{
		List<string> paths = new List<string> ();

		foreach (string file in Directory.GetDirectories (PathURL))
		{
			foreach (string child in Directory.GetFiles (file))
			{
				if (child.Contains (".meta") == false)
				{
					paths.Add ("file://" + child);
					//Debug.Log ("file://" + child);
				}
			}
		}
		return paths;
	}

	private IEnumerator LoadAssetRes ()
	{
		float last = Time.time;
		DebugConsole.Log ("<----Start Init Resource--->");
		List<string> paths = GetPathName ();
		DebugConsole.Log (paths.Count);
		if (paths.Count > 0)
		{
			foreach (string p in paths)
			{
				WWW bundle = new WWW(p);  
				yield return bundle;
				if (bundle.error == null)
				{
					//assetBundles.Add (p, bundle.assetBundle);
					GameObject prefab = (GameObject) bundle.assetBundle.mainAsset;
					prefabs.Add (prefab.name, prefab);
					bundle.assetBundle.Unload (false);
					//Debug.Log (prefabs[prefab.name]);
					bundle.Dispose();
					bundle = null;
				}
				else
				{
					Debug.LogError (p + "Load error<-------------->" + bundle.error );
					DebugConsole.LogError (p + "Load error");
				}
			}
			//Debug.Log ();
//			foreach (KeyValuePair<string, AssetBundle> kvp in assetBundles)
//			{
//				Debug.Log (kvp.Value + "______" + kvp.Key);
//			}
			DebugConsole.Log ("<----Init Resource Success--->Cost Time" + (Time.time - last));
			isSuccess = true;
			if (loadAssetComplete != null)
			{
				loadAssetComplete ();
			}
		} 
	}


	public GameObject getIdByPrefabs (string id)
	{
		return prefabs[id];
	}

	public void clear ()
	{
		prefabs.Clear ();
		Resources.UnloadUnusedAssets();
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (400,0, 100, 100), "Load Next"))
		{  
			clear ();
			Application.LoadLevel ("PublicZone");
		}  
	}

	/*IEnumerator delayDownload ()
	{
		WWW www = new WWW ("ftp://qq459127484:kanni789@002.3vftp.com/Users.txt");
		yield return www;
		Debug.Log (www.progress);
		if (www.error == null)
		{
			if (www.isDone)
			{
				Debug.Log (www.text);
			}
		}
		else
		{
			Debug.Log ("Download error");
		}
	}*/
}
