﻿using UnityEngine;
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

	[MenuItem("Custom Editor/Create AssetBunldes Common")]  
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
			string file = obj.name.Substring (0,2);
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

	[MenuItem("Custom Editor/Create Scenes")]
	static void CreateScenesMain ()  
	{
		Caching.CleanCache();
		BuildTarget bt = BuildTarget.StandaloneWindows;
#if UNITY_ANDROID   //安卓  
		bt = BuildTarget.Android;
#elif UNITY_IPHONE  //iPhone  
		bt = BuildTarget.iPhone;
#endif

		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);  
		foreach (Object obj in SelectedAsset)   
		{
			string file = "SC";
			string targetPath = Application.dataPath + "/A_MarsRes/" + PathURL + "/" + file;
			DirectoryInfo dict = new DirectoryInfo(targetPath);

			if (dict.Exists == false)
			{
				string path =  AssetDatabase.CreateFolder ("Assets/A_MarsRes/" + PathURL, file);
			}
			targetPath += "/" + obj.name + ".unity3d";  
			string  []levels = {string.Format ("Assets/Scenes/{0}.unity", obj.name)};//set scene who need
			//Debug.Log (levels[0]);
			BuildPipeline.BuildPlayer( null, targetPath,bt, BuildOptions.BuildAdditionalStreamedScenes);
			Debug.Log (targetPath);
			//Clear
//			
//			string Path = Application.dataPath + "/Scenes.unity3d";
//			string  []levels = {"Assets/Scenes/test.unity"};//set scene who need
//			//scene package
//			BuildPipeline.BuildPlayer( levels, Path,BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
		}
			AssetDatabase.Refresh ();
	}
}
