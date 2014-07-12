using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CreatAssetBundle : Editor {

	public static readonly string PathURL =  
#if UNITY_ANDROID   //安卓  
	"Android";
#elif UNITY_IPHONE  //iPhone  
	"IOS";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"PC";
#else  
	string.Empty;  
#endif  

	[MenuItem("Game Editor/Create AssetBunldes Common")]  
	static void CreateAssetBunldesMain ()  
	{  
		BuildTarget bt = BuildTarget.StandaloneWindows;
#if UNITY_ANDROID   //安卓  
		bt = BuildTarget.Android;
#elif UNITY_IPHONE  //iPhone  
		bt = BuildTarget.iPhone;
#endif
		
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{  
			string sourcePath = AssetDatabase.GetAssetPath (obj);
			string[] paths = sourcePath.Split ('/');
			string file = paths[paths.Length - 2];
			Debug.Log (file);
			//string file = obj.name.Substring (0,2);
			string targetPath = Application.dataPath + "/A_MarsRes/" + PathURL + "/" + file;
			DirectoryInfo dict = new DirectoryInfo(targetPath);

			if (dict.Exists == false)
			{
				string path =  AssetDatabase.CreateFolder ("Assets/A_MarsRes/" + PathURL, file);

			}
			targetPath += "/" + obj.name + ".assetbundle";  
			Debug.Log (bt);
			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies, bt)) {  
				Debug.Log(obj.name +"Creat success");  
			}   
			else   
			{  
				Debug.Log(obj.name +"Creat fail");  
			}
		}  
		AssetDatabase.Refresh ();     
	}  

	[MenuItem("Game Editor/Create Scenes")]
	static void CreateScenesMain ()  
	{
		Caching.CleanCache();
		BuildTarget bt = BuildTarget.StandaloneWindows;
#if UNITY_ANDROID   //安卓  
		bt = BuildTarget.Android;
#elif UNITY_IPHONE  //iPhone  
		bt = BuildTarget.iPhone;
#endif

		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{
			string sourcePath = AssetDatabase.GetAssetPath (obj);


			if (sourcePath.Contains (".unity"))
			{
				string file = "SC";
				string targetPath = Application.dataPath + "/A_MarsRes/" + PathURL + "/" + file;
				DirectoryInfo dict = new DirectoryInfo(targetPath);

				if (dict.Exists == false)
				{
					string path =  AssetDatabase.CreateFolder ("Assets/A_MarsRes/" + PathURL, file);
				}
				targetPath += "/" + obj.name + ".unity3d";  

				//string navMeshPath = sourcePath.Replace (".unity", "");
				string[] levels = { sourcePath };//set scene who need
				//bool isExit = File.Exists (navMeshPath);
//				if (isExit)
//				{
//					levels = new string[1]{ navMeshPath };
//				}
				//Debug.LogError ();//NavMesh.asset


				BuildPipeline.BuildPlayer( levels, targetPath,bt, BuildOptions.BuildAdditionalStreamedScenes);
				Debug.Log (targetPath);
			}
		}
			AssetDatabase.Refresh ();
	}

	[MenuItem("Game Editor/Create MonsterPos", false, 9)]
	static void CreatMonsterPosJson ()
	{
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		try
		{
			string sourcePath = AssetDatabase.GetAssetPath (SelectedAsset[0]);
			if (sourcePath.Contains (".unity"))
			{
				string targetPath = sourcePath.Replace (".unity", ".txt");
				Fight fight = new Fight ();
				fight.gameMonsters = new Dictionary<string, GameMonster[]> ();
				for (int i = 100000;;i++)
				{
					string tra_Name = i.ToString ();
					GameObject go = GameObject.Find (tra_Name);
					if (go != null)
					{

						Transform json_Go = go.transform.FindChild ("MonsterPos");
						fight.gameMonsters.Add (tra_Name, new GameMonster[json_Go.childCount]);
						int idx = 0;
						foreach (Transform tra in json_Go)
						{
							string[] arr = tra.name.Split ('_');
							GameMonster gameMonster = new GameMonster ();
							gameMonster.id = int.Parse(arr[0]);
							gameMonster.type = arr[1];
							gameMonster.level = int.Parse (arr[2]);
							gameMonster.x = tra.position.x;
							gameMonster.z = tra.position.z;
							fight.gameMonsters[tra_Name][idx] = gameMonster;
							idx++;

						}
					}
					else break;
				}


				FileStream fs = new FileStream (targetPath, FileMode.Create, FileAccess.Write);
				StreamWriter sw = new StreamWriter(fs);
				sw.WriteLine (JsonConvert.SerializeObject (fight));
				sw.Close ();
				fs.Close ();
				Debug.Log ("CREAT " + targetPath + " SUCCESSFUL");
				AssetDatabase.Refresh ();
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError (e);
		}

	}
}
