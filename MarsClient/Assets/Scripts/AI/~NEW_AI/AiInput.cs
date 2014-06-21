using UnityEngine;
using System.Collections;

public class AiInput : MonoBehaviour {

	private AiPlayer aiPlayer;
	private AiMove aiMove;

	void Start () 
	{
		aiPlayer = GetComponent <AiPlayer> ();
		aiMove = GetComponent<AiMove> ();
	}

	void Update ()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		//move
		float x = Input.GetKey (KeyCode.A) ? 1 : Input.GetKey (KeyCode.D) ? -1 : 0 ;
		float z = Input.GetKey (KeyCode.S) ? 1 : Input.GetKey (KeyCode.W) ? -1 : 0 ;
#elif UNITY_ANDROID || UNITY_IPHONE
		float x = -Joystick.getPosition.x;
		float z = -Joystick.getPosition.y;
#endif
		UpdateMove (new Vector3 (x, 0, z));
		//attack
		if (Input.GetMouseButton (0) && UISceneLoading.currentLoadName != UISceneLoading.PUBLIC_ZONE)
		{
			aiPlayer.NormalAttack ();
		}
	}
#if UNITY_ANDROID || UNITY_IPHONE
	public void MobliePlatform ()
	{
		aiPlayer.NormalAttack ();
	}
#endif
	public void UpdateMove (Vector3 dir)
	{
		aiMove.UpdateMove (dir);
	}

	void OnGUI ()
	{
//		if (GUILayout.Button ("Spell1"))
//		{
//			aiPlayer.ShootSpell1 ();
//		}
//		if (GUILayout.Button ("Spell2"))
//		{
//			aiPlayer.ShootSpell2 ();
//		}
	}
}
