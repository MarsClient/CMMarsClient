using UnityEngine;
using System.Collections;

public class JoyStick : MonoBehaviour {


	private static JoyStick instance;
	public static Vector2 position
	{
		get
		{
			if (instance == null)
			{
				GameObject res_Go = Resources.Load ("Joystick", typeof (GameObject)) as GameObject;
				GameObject go = GameObject.Instantiate (res_Go) as GameObject;
			}
			return instance.m_postion;
		}
	}


	public float MaxOffset = 50;
	public Transform joystickTra;
	private Vector2 m_postion;


	void Awake ()
	{
		instance = this;
	}

	void OnPress (bool isPress)
	{
		if (isPress)
		{
			transform.position = UICamera.lastHit.point;
			InvokeRepeating ("UpdateJoystickOperator", 0, 0.03333f);
		}
		else
		{
			Clear ();
			CancelInvoke ("UpdateJoystickOperator");
		}
	}

	void UpdateJoystickOperator ()
	{
		joystickTra.position = UICamera.lastHit.point;
		if (Vector3.Distance (Vector3.zero, joystickTra.localPosition) >= MaxOffset)
		{
			joystickTra.localPosition = joystickTra.localPosition.normalized * MaxOffset;
		}
		m_postion = joystickTra.localPosition.normalized;

	}

	void Clear ()
	{
		m_postion = Vector3.zero;
		joystickTra.localPosition = Vector3.zero;
		transform.localPosition = Vector3.zero;
	}
}
