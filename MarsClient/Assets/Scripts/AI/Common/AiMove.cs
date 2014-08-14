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
	private float factSpeed = 0;
	
	//ai controller
	private CharacterController characterController;
	
	// ai move dir
	private Vector3 m_dir = Vector3.zero;
	
	void Awake () 
	{
		characterController = GetComponent <CharacterController>();
		SetMoveSpeed ();
		StartUpdate ();
	}
	
	public void SetMoveSpeed () { SetMoveSpeed (1); }
	public void SetMoveSpeed (float ratio) { factSpeed = moveSpeed * ratio; }
	
	public void StartUpdate ()
	{
		InvokeRepeating ("SpecialMoving", 0, 0.03333f);
	}
	
	public void UpdateMove (Vector3 move) 
	{
		if (_currentMoveState != MoveState.SpecialMoving)
		{
			if (isPlayer == true)
			{
				m_dir = move;
				_currentMoveState = (move.x != 0 || move.z != 0) ? MoveState.Moving : MoveState.Stop;
				CallbackMoveEvent (this);
				SetMove (m_dir, factSpeed);
			}
		}
		else
		{
			SpecialMoving ();
		}
	}
	
	public void SpecialMoving ()
	{
		if (_currentMoveState == MoveState.SpecialMoving)
		{
			float distance = Vector3.Distance (transform.position, startPos);
			if (distance < moveDistance)
			{
				//Debug.LogError (distance + "___" + moveDistance);
				CollisionFlags cf = SetMove (transform.forward, speed);
				if (Time.time - m_startTime < m_LastTime)
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
		Vector3 pos = transform.position;
		pos.y = 0;
		transform.position = pos;
		return characterController.Move (dir * spd * Time.deltaTime);
	}
	
	
	private ControllerColliderHit m_Hit;
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		this.m_Hit = hit;
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
	
	
	private float m_LastTime = 0;
	private float m_startTime = 0;
	public void startMoveDir (AnimationInfo info, FrameEvent fe/*, bool isForward = true*/)
	{
		if (fe.antDisatnce == 0)
		{
			return;
		}
		m_Hit = null;
		isForward = fe.antDisatnce > 0;
		_currentMoveState = MoveState.SpecialMoving;
		startPos = transform.position;
		isForward = isForward;
		this.moveDistance = Mathf.Abs (fe.antDisatnce);
		this.speed = fe.antMoveSpd;
		this.currentAnt = info;
		
		this.m_LastTime = Mathf.Abs (moveDistance / speed);
		m_startTime = Time.time;
	}
}
