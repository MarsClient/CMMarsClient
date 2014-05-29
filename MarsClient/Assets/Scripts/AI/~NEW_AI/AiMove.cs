using UnityEngine;
using System.Collections;

public class AiMove : MonoBehaviour {

	//set move event
	public delegate void MoveEvent (AiMove move);
	public MoveEvent moveEvent;


	public enum MoveState { Stop, Moving, SpecialMoving, }

	private MoveState _currentMoveState;
	public MoveState currentMoveState
	{
		set { if (value == MoveState.Stop) { m_dir = Vector3.zero; } _currentMoveState = value; }
		get { return _currentMoveState; }
	}

	public bool isPlayer = true;
	//ai move speed
	public float moveSpeed = 3;

	//ai controller
	private CharacterController characterController;

	// ai move dir
	private Vector3 m_dir = Vector3.zero;

	void Start () 
	{
		characterController = GetComponent <CharacterController>();
	}

	void Update () 
	{
		if (_currentMoveState != MoveState.SpecialMoving)
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			float x = Input.GetKey (KeyCode.A) ? 1 : Input.GetKey (KeyCode.D) ? -1 : 0 ;
			float z = Input.GetKey (KeyCode.S) ? 1 : Input.GetKey (KeyCode.W) ? -1 : 0 ;
			m_dir = new Vector3 (x, 0, z);
#endif
			_currentMoveState = (x != 0 || z != 0) ? MoveState.Moving : MoveState.Stop;
		}
		CallbackMoveEvent (this);
		SetMove (m_dir);
	}

	CollisionFlags SetMove (Vector3 dir)
	{
		dir = dir.normalized;
		if (dir != Vector3.zero)
		{
			transform.forward = dir;
		}
		return characterController.Move (dir * moveSpeed * Time.deltaTime);
	}

	void CallbackMoveEvent (AiMove aiMove)
	{
		if (moveEvent != null)
		{
			moveEvent (aiMove);
		}
	}
}
