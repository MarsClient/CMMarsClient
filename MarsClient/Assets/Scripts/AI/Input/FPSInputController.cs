using UnityEngine;
using System.Collections;


//[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {


	public CharacterController characterController;

	private Vector3 m_directionVector;
	public Vector3 directionVector
	{
		get
		{
			return m_directionVector;
		}
		set
		{
			m_directionVector = value;
		}
	}
	private CharacterMotor m_motor ;
	public CharacterMotor motor
	{
		get
		{
			return m_motor;
		}
	}
	public bool canControl;


	public delegate void InputController (FPSInputController fps);
	public static InputController inputController;

	public delegate void AttackController (FPSInputController fps);
	public static AttackController attackController;

	public delegate void AssaultDelegate (AnimationItem animationItem);
	public static AssaultDelegate assaultDelegate;


	public Vector2 dir;

	// Use this for initialization
	void Awake () {
		//m_motor = GetComponent<CharacterMotor>();
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isMoveDir == false)
		{
			// Get the input vector from kayboard or analog stick
			float x = Input.GetKey (KeyCode.A) ? dir.x : Input.GetKey (KeyCode.D) ? -dir.x : 0 ;
			float z = Input.GetKey (KeyCode.S) ? dir.y : Input.GetKey (KeyCode.W) ? -dir.y : 0 ;
			//Debug.Log (x + "___" + y);
			m_directionVector = new Vector3(x, 0, z);
			//Debug.Log (m_directionVector);
			if (canControl == false)
			{
				m_directionVector = Vector3.zero;
			}
			if (m_directionVector != Vector3.zero) 
			{
				if (canControl == true)
				{
					transform.forward = m_directionVector;
				}
			}
			//DebugConsole.Log (m_directionVector);
			characterController.Move (m_directionVector.normalized * 0.08f);
			//m_motor.inputMoveDirection = m_directionVector.normalized;

			if (inputController != null)
			{
				inputController (this);
			}

			if (Input.GetMouseButton (0) || Input.GetKeyDown(KeyCode.J))
			{
				if (attackController != null)
				{
					attackController (this);
				}
			}
		}
		//********Dir move
		else// (m_isMoveDir)
		{
			if (Vector3.Distance (transform.position, startPos) < moveDistance)
			{
				CollisionFlags cf = characterController.Move (transform.forward * Time.deltaTime * speed);
				if (cf == CollisionFlags.None)
				{
					if (assaultDelegate != null)
					{
						assaultDelegate (currentAnt);
					}
					return;
				}
			}
			m_isMoveDir = false;
		}
	}

	private bool m_isMoveDir = false;
//	private float lastTime = 0;
//	private float timeing = 0.05f;
	private float moveDistance;
	private AnimationItem currentAnt;
	private float speed = 0;
	private Vector3 startPos;
	private bool isForward = true;
	//private Vector3 m_dir;
	public void moveDir (AnimationItem ai, bool isForward = true)
	{
		if (ai.actionMove == 0)
		{
			return;
		}
		m_isMoveDir = true;
		isForward = isForward;
		this.moveDistance = ai.actionMove;
		this.currentAnt = ai;
	}
	
}