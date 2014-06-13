using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using DC = DebugConsole;

public class GameData	{

	public const int DbVersion = 1;

	private string LoginDbUrl (string name)
	{
		//TODO:
		return Main.Instance.sqliteVer.url;// + name;
	}

	private Dictionary<string, Troop> enemys = new Dictionary<string, Troop> ();//enemys
	private Dictionary<string, Troop> players = new Dictionary<string, Troop> ();//player
	private Dictionary<string, Spell> spells = new Dictionary<string, Spell>();//spells

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

		LoadTroops (db);
		LoadPlayers (db);
		LoadSpells (db);

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

	void LoadTroops (SQLiteDB db)
	{
		enemys.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM Troops"); 
		while (qr.Step ())
		{
			Troop troop = new Troop ();
			troop.id = qr.GetString ("id");
			troop.level = qr.GetInteger ("level");
			troop.type = qr.GetString ("type");
			troop.assetbundle = qr.GetString ("assetbundle");
			troop.icon = qr.GetString ("icon");
			troop.desc = qr.GetString ("desc");
			troop.name = qr.GetString ("name");
			troop.dmg = (float) qr.GetDouble ("dmg");
			troop.def = (float) qr.GetDouble ("def");
			troop.hpMax = (float) qr.GetDouble ("hpMax");
			troop.hp = troop.hpMax;
			//DC.LogError (JsonConvert.SerializeObject(troop));
			enemys.Add (troop.id, troop);
		}
	}

	void LoadPlayers (SQLiteDB db)
	{
		players.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM Players"); 
		while (qr.Step ())
		{
			Troop troop = new Troop ();
			troop.id = qr.GetString ("id");
			troop.level = qr.GetInteger ("level");
			troop.type = qr.GetString ("type");
			troop.assetbundle = qr.GetString ("assetbundle");
			troop.icon = qr.GetString ("icon");
			troop.desc = qr.GetString ("desc");
			troop.name = qr.GetString ("name");
			troop.dmg = (float) qr.GetDouble ("dmg");
			troop.def = (float) qr.GetDouble ("def");
			troop.hpMax = (float) qr.GetDouble ("hpMax");
			troop.hp = troop.hpMax;
			//DC.Log (JsonConvert.SerializeObject(troop));
			players.Add (troop.id, troop);
		}
	}

	void LoadSpells (SQLiteDB db)
	{
		spells.Clear ();
		SQLiteQuery qr = new SQLiteQuery(db, "SELECT * FROM Spells"); 
		while (qr.Step ())
		{
			Spell spell = new Spell ();
			spell.id = qr.GetString ("id");
			spell.level = qr.GetInteger ("level");
			spell.type = qr.GetString ("type");
			spell.assetbundle = qr.GetString ("assetbundle");
			spell.icon = qr.GetString ("icon");
			spell.desc = qr.GetString ("desc");
			spell.name = qr.GetString ("name");
			spell._dmg = (float) qr.GetDouble ("dmg");
			spell.pro = qr.GetString ("pro");
			spell.act = qr.GetString ("act");
			spell.cd = (float) qr.GetDouble ("cd");
			//DC.Log (JsonConvert.SerializeObject(spell));
			spells.Add (spell.id, spell);
		}
	}

	public Troop getPlayerById (string id)
	{
		if (players.Count > 0)
		{
			Troop troop = null;
			bool isFind = players.TryGetValue (id, out troop);
			if (isFind)
			{
				return troop;
			}
		}
		return null;
	}

	public List<Troop> getTroops ()
	{
		List<Troop> troops = new List<Troop> ();
		foreach (KeyValuePair<string, Troop> kvp in enemys)
		{
			troops.Add (kvp.Value);
		}
		return troops;
	}

	public List<Spell> getProSpells (string pro)
	{
		List<Spell> proSpells = new List<Spell>();
		foreach (KeyValuePair<string, Spell> kvp in spells)
		{
			if (kvp.Value.pro == pro)
			{
				proSpells.Add (kvp.Value);
			}
		}
		return proSpells;
	}

	public Spell getSpByIdPro (string pro, string act)
	{
		List<Spell> spell = new List<Spell>();
		spell = getProSpells (pro);
		foreach (Spell sp in spell)
		{
			if (sp.act == act)
			{
				return sp;
			}
		}
		Spell s = new Spell();
		s._dmg = 1;
		return s;
	}
}
