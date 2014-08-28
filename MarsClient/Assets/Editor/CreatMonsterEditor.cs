using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[CustomEditor (typeof (CreatMonster))]
public class CreatMonsterEditor : Editor {

	/*
	 *@see script.id is tranform's name
	 *@see script.x is tranform.pos.x
	 *@see script.z is tranform.pos.z
	*/
	public override void OnInspectorGUI () 
	{
		CreatMonster script = target as CreatMonster;

		DrawDefaultInspector ();
		EditorGUI.indentLevel = 1;

		script.id = script.transform.name;//EditorGUILayout.LabelField ("id", script.transform.name);
		script.x = script.transform.position.x;
		script.z = script.transform.position.z;

		if (GUILayout.Button ("Modified spells is invalid, when isBoss is False!!!","HelpBox"))
		{}

		if (GUI.changed) EditorUtility.SetDirty (target);
	}



	/*
	 *@Creat operator
	 */
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
						foreach (CreatMonster creatMonster in json_Go.GetComponentsInChildren<CreatMonster>())
						{

							GameMonster gameMonster = new GameMonster ();
							gameMonster.id = creatMonster.id;
							gameMonster.type = creatMonster.type;
							gameMonster.level = creatMonster.level;
							gameMonster.x = creatMonster.x;
							gameMonster.z = creatMonster.z;
							gameMonster.hp = creatMonster.hpMax;
							gameMonster.hpMax = creatMonster.hpMax;
							gameMonster.isBoss = creatMonster.isBoss;
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
