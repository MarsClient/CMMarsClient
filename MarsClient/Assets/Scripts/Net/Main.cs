using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	public static Main Instance;

	public SQLiteVer sqliteVer;
	public Account account;

	void Awake ()
	{
		Instance = this;
		//StartCoroutine (GameData.Instance.reload ());
	}
}
