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
	
}