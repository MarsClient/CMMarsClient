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

	public void UpdateMove (Vector3 move) 
	{
		if (_currentMoveState != MoveState.SpecialMoving)
		{

			m_dir = move;
			_currentMoveState = (move.x != 0 || move.z != 0) ? MoveState.Moving : MoveState.Stop;
			CallbackMoveEvent (this);
			SetMove (m_dir, moveSpeed);
		}
		else
		{
			if (Vector3.Distance (transform.position, startPos) < moveDistance)
			{
				CollisionFlags cf = SetMove (transform.forward, speed);
				if (cf == CollisionFlags.None)
				{
					return;
				}
			}
			currentAnt = null;
			_currentMoveState = MoveState.Stop;
		}
	}

	CollisionFlags SetMove (Vector3 dir, float spd)
	{
		dir = dir.normalized;
		if (dir != Vector3.zero)
		{
			transform.forward = dir;
		}
		return characterController.Move (dir * spd * Time.deltaTime);
	}

	void CallbackMoveEvent (AiMove aiMove)
	{
		if (moveEvent != null)
		{
			moveEvent (aiMove);
		}
	}

	private float moveDistance;
	private AnimationInfo currentAnt;
	private float speed = 1;
	private Vector3 startPos;
	private bool isForward = true;
	public void startMoveDir (AnimationInfo info, FrameEvent fe/*, bool isForward = true*/)
	{
		if (fe.antDisatnce == 0)
		{
			return;
		}
		isForward = fe.antDisatnce > 0;
		_currentMoveState = MoveState.SpecialMoving;
		startPos = transform.position;
		isForward = isForward;
		this.moveDistance = Mathf.Abs (fe.antDisatnce);
		this.speed = fe.antMoveSpd;
		this.currentAnt = info;
		Debug.Log ("Call");
	}
}
