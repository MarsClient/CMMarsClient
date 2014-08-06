using UnityEngine;
using System.Collections;

/// <summary>
/// All game only one
/// </summary>
public class AiInput : MonoBehaviour {

	public static AiInput instance;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			enabled = false;
		}
	}

	public void OnDestroy ()
	{
		instance = null;
	}


	private AiPlayer aiPlayer;
	private AiMove aiMove;

	void Start () 
	{
		aiPlayer = GetComponent <AiPlayer> ();
		aiMove = GetComponent<AiMove> ();
	}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
	void Update ()
	{
		float x = Input.GetKey (KeyCode.A) ? 1 : Input.GetKey (KeyCode.D) ? -1 : 0 ;
		float z = Input.GetKey (KeyCode.S) ? 1 : Input.GetKey (KeyCode.W) ? -1 : 0 ;
		UpdateMove (new Vector3 (x, 0, z));
		//attack
		if ((Input.GetMouseButton (0) || Input.GetKey (KeyCode.J)) && ScenesManager.currentLoadName != ScenesManager.PUBLIC_ZONE)
		{
			NormalAttack ();
		}
	}
#endif

	public void NormalAttack ()
	{
		aiPlayer.NormalAttack ();
	}

	public void UpdateMove (Vector3 dir)
	{
		aiMove.UpdateMove (dir);
	}

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
	void OnGUI ()
	{
		if (GUILayout.Button ("Spell1"))
		{
			aiPlayer.ShootSpell1 ();
		}
		if (GUILayout.Button ("Spell2"))
		{
			aiPlayer.ShootSpell2 ();
		}
		if (GUILayout.Button ("Spell3"))
		{
			aiPlayer.ShootSpell3 ();
		}
	}
//#endif
}
