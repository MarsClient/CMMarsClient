using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using DC = DebugConsole;

public class GameData	{

	public const int DbVersion = 1;

	public bool isLoadingSuccess = false;

	private string LoginDbUrl (string name)
	{
		//TODO:
		return Main.Instance.sqliteVer.url;// + name;
	}

	private Dictionary<long, GameItem> gameItems = new Dictionary<long, GameItem>();
	private Dictionary<long, GameSpell> gameSpells = new Dictionary<long, GameSpell>();

	private Dictionary<long, GameNPC> gameNPCs = new Dictionary<long, GameNPC>();//key is id
	private Dictionary<string, GameNPC> gameNPCsModel = new Dictionary<string, GameNPC>();

	private Dictionary <int, Dictionary <int, GameEffect>> gameEffects = new Dictionary<int, Dictionary<int, GameEffect>>();


	private Dictionary <string, string> gameStrings = new Dictionary<string, string> ();

	public GameData ()
	{}

	private static GameData _Instance;
	public static GameData Instance
	{
		get{
			if (_Instance == null)
			{
				_Instance = new GameData ();
			}
			return _Instance;
		}
	}

	public IEnumerator reload()
	{
		//Debug.Log ("Open");
		return OpenDB("MG.db");
	}

	IEnumerator OpenDB(string name)
	{
		SQLiteDB db = new SQLiteDB ();
		string log = "";
		string fileName = Application.persistentDataPath + name;
//		Debug.LogError (fileName);
		// check if database already exists.
		if(!File.Exists(fileName))
		{
			DC.Log ("Open URL DB");
			bool success = false;
			do
			{
				// ok , this is first time application start!
				// so lets copy prebuild dtabase from web and load store to persistancePath
				yield return new WaitForSeconds(1.0f);
				using (var www = new WWW(LoginDbUrl (name)))
				{
					while (!www.isDone)
					{
						yield return www;
					}
					if (www.error != null)
					{
						Debug.Log(www.error +"\n"+ LoginDbUrl (name));
					}
					else
					{
						success = writeFile(fileName, www.bytes);
					}
				}
			} while (success == false);
		}
		if(!File.Exists(fileName))
		{
			DC.Log ("Open LOCAL DB");
			loadLocalDb (name, fileName);
		}
		// it mean we already download prebuild data base and store into persistantPath
		// lest update, I will call Test
		try{
			//
			// initialize database
			//
			db.Open(fileName);
			log += "\nDatabase opened! filename:"+fileName;
		} catch (Exception e){
			log += 	"\nTest Fail with Exception " + e.ToString();
			log += 	"\n on WebPlayer it must give an exception, it's normal.";
		}
		int version = getVersion(db);
//		Debug.Log ("OLD***********" + version);

		if (Main.Instance.sqliteVer.ver > version)
		{
			db.Close();
			bool success = false;
			do
			{
				// ok , this is first time application start!
				// so lets copy prebuild dtabase from web and load store to persistancePath
				yield return new WaitForSeconds(1.0f);
				using (var www = new WWW(LoginDbUrl (name)))
				{
					while (!www.isDone)
					{
						yield return www;
					}
					if (www.error != null)
					{
						Debug.Log(www.error +"\n"+ LoginDbUrl (name));
					}
					else
					{
						success = writeFile(fileName, www.bytes);
					}
				}
			} while (success == false);
			try{
				//
				// initialize database
				//
				db.Open(fileName);
				log += "\nDatabase opened! filename:"+fileName;
			} catch (Exception e){
				log += 	"\nTest Fail with Exception " + e.ToString();
				log += 	"\n on WebPlayer it must give an exception, it's normal.";
			}
			//Debug.Log ("NEW***********" + getVersion(db));
		}

		LoadGameItme (db);
		LoadGameSpell (db);
		LoadGameNpc (db);
		LoadGameStrings (db);
		LoadGameEffects (db);
		isLoadingSuccess = true;
		UISceneLoading.instance.DelaySuccessLoading ();
		db.Close ();
	}

	void loadLocalDb(string dbfilename, string saveFilename)
	{
		byte[] bytes = null;
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		string dbpath = "file://" + Application.streamingAssetsPath + "/" + dbfilename;
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
#elif UNITY_WEBPLAYER
		string dbpath = "StreamingAssets/" + dbfilename;
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
#elif UNITY_IPHONE
		string dbpath = Application.dataPath + "/Raw/" + dbfilename;
		try{	
			using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
				bytes = new byte[fs.Length];
				fs.Read(bytes,0,(int)fs.Length);
			}			
		} catch (Exception e){
			Debug.Log("Test Fail with Exception " + e.ToString());
		}
#elif UNITY_ANDROID
		string dbpath = Application.streamingAssetsPath + "/" + dbfilename;
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		#endif
		if (bytes != null)
		{
			writeFile(saveFilename, bytes);
		}
	}
	
	IEnumerator Download( WWW www )
	{
		yield return www;
		
		while (!www.isDone)
		{
			
		}
	}
	
	int getVersion(SQLiteDB db)
	{
		SQLiteQuery qr = new SQLiteQuery(db, "PRAGMA user_version"); 
		while( qr.Step() )
		{
			return qr.GetInteger("user_version");
		}
		return 0;
	}

	bool writeFile(string filename, byte[] bytes)
	{
		if ( bytes != null && bytes.Length > 0)
		{
			try
			{
				File.Delete(filename);
				// copy database to real file into cache folder
				using( FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write) )
				{
					fs.Write(bytes,0,bytes.Length);
					return true;
				}
			}
			catch (System.Exception e)
			{
				Debug.Log(e.ToString());
			}
		}
		return false;
	}

	void LoadGameItme (SQLiteDB db)
	{
		gameItems.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM GameItem");
		while (qr.Step ())
		{
			GameItem gameItem = new GameItem ();
			gameItem.id = long.Parse (qr.GetString ("id"));
			gameItem.type = qr.GetString ("type");
			gameItem.func = qr.GetString ("func");
			gameItem.belong = qr.GetString ("belong");
			gameItem.model = qr.GetString ("model");
			gameItem.cd = qr.GetInteger ("cd");
			gameItem.icon = qr.GetString ("icon");
			gameItem.desc = qr.GetString ("desc");
			gameItem.name = qr.GetString ("name");
			gameItem.value = qr.GetInteger ("value");
			gameItem.gold = qr.GetInteger ("gold");
			gameItem.gem = qr.GetInteger ("gem");
			gameItems.Add (gameItem.id, gameItem);
		}
	}

	void LoadGameSpell (SQLiteDB db)
	{
		gameSpells.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM GameSpell");
		while (qr.Step ())
		{
			GameSpell gameSpell = new GameSpell ();
			gameSpell.id = long.Parse  (qr.GetString ("id"));
			gameSpell.type = qr.GetString ("type");
			gameSpell.func = qr.GetString ("func");
			gameSpell.belong = qr.GetString ("belong");
			gameSpell.model = qr.GetString ("model");
			gameSpell.cd = qr.GetInteger ("cd");
			gameSpell.icon = qr.GetString ("icon");
			gameSpell.desc = qr.GetString ("desc");
			gameSpell.name = qr.GetString ("name");
			gameSpell.pro = qr.GetString ("pro");
			gameSpell.shoottype = qr.GetInteger ("shoottype");
			gameSpells.Add (gameSpell.id, gameSpell);
		}
	}

	void LoadGameNpc (SQLiteDB db)
	{
		gameNPCs.Clear ();
		gameNPCsModel.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM GameNPC");
		while (qr.Step ())
		{
			GameNPC gameNPC = new GameNPC ();
			gameNPC.id = long.Parse  (qr.GetString ("id"));
			gameNPC.type = qr.GetString ("type");
			gameNPC.model = qr.GetString ("model");
			gameNPC.icon = qr.GetString ("icon");
			gameNPC.desc = qr.GetString ("desc");
			gameNPC.name = qr.GetString ("name");
			gameNPC.talkNum = qr.GetInteger ("talkNum");
			gameNPC.region = qr.GetInteger ("region");
			gameNPC.x = (float) qr.GetDouble ("x");
			gameNPC.z = (float) qr.GetDouble ("z");
			gameNPC.roY = (float) qr.GetDouble ("roY");

			gameNPCsModel.Add (gameNPC.model, gameNPC);
			gameNPCs.Add (gameNPC.id, gameNPC);
		}
	}

	void LoadGameStrings (SQLiteDB db)
	{
		gameStrings.Clear ();
		//gameNPCsModel.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM GameString");
		while (qr.Step ())
		{
			string key = qr.GetString ("key");
			string value = qr.GetString ("value");
			gameStrings.Add (key, value);

		}
	}

	void LoadGameEffects (SQLiteDB db)
	{
		gameEffects.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM GameEffect");
		while (qr.Step ())
		{
			GameEffect gameEffect = new GameEffect ();
			gameEffect.id = qr.GetInteger ("id");
			try
			{
				gameEffect.assetbundle = qr.GetString ("assetbundle");
			}
			catch (System.Exception e)
			{
				continue;
			}
			gameEffect.fxType = (FxType) qr.GetInteger ("index");
			gameEffect.action = qr.GetInteger ("action");
			if (gameEffects.ContainsKey (gameEffect.id) == false)
			{
				gameEffects[gameEffect.id] = new Dictionary<int, GameEffect> ();
			}
			gameEffects[gameEffect.id].Add (gameEffect.action, gameEffect);
		}
	}

	public string[] getAllNpcsModel ()//get path
	{
		string[] models = new string[gameNPCs.Count];
		int i = 0;
		foreach (KeyValuePair<long, GameNPC> kvp in gameNPCs)
		{
			models[i++] = Constants.NPC +  kvp.Value.model;
		}
		return models;
	}

	public GameNPC getNpcByModel (string model)
	{
		GameNPC npc = null;
		gameNPCsModel.TryGetValue (model, out npc);
		return npc;
	}

	public string getLocalString (string key)
	{
		string str = key;
		gameStrings.TryGetValue (key, out str);
		return str;
	}

	public GameEffect getGameEffectByAction (int pro, int action)
	{
		GameEffect gameEffect = null;
		if (gameEffects [pro] != null)
		{
			gameEffects [pro].TryGetValue (action, out gameEffect);
		}
		return gameEffect;
	}
}
