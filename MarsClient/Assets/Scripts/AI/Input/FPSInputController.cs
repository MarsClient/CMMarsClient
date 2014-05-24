using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {
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


	public Vector2 dir;

	// Use this for initialization
	void Awake () {
		m_motor = GetComponent<CharacterMotor>();
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
			m_motor.inputMoveDirection = m_directionVector;

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
				CollisionFlags cf = motor.characterController.Move (transform.forward * Time.deltaTime * 20);
				if (cf == CollisionFlags.None)
					return;
			}
			m_isMoveDir = false;
		}
	}

	private bool m_isMoveDir = false;
	private float lastTime = 0;
	private float timeing = 0.05f;
	private float moveDistance;
	private Vector3 startPos;
	private bool isForward = true;
	//private Vector3 m_dir;
	public void moveDir (float moveDistance, bool isForward = true)
	{
		if (moveDistance == 0)
		{
			return;
		}
		lastTime = Time.time;
		m_isMoveDir = true;
		isForward = isForward;
		startPos = transform.position;
		this.moveDistance = moveDistance;
	}
	
}