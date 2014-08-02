using UnityEngine;
using System.Collections;

public class JoyStick : UIButtonLong {


	private static JoyStick instance;
	public static Vector2 position
	{
		get
		{
			return instance.m_postion;
		}
	}


	public float MaxOffset = 50;
	public Transform joystickTra;
	private Vector2 m_postion;
	private UIRoot root;

	private Transform referToTra;

	void Awake ()
	{
		instance = this;

		root = NGUITools.FindInParents<UIRoot> (gameObject);
		GameObject go = new GameObject("referToTra");
		referToTra = go.transform;
		referToTra.parent = root.transform;
		referToTra.localScale = Vector3.one;
		referToTra.localPosition = Vector3.zero;
	}

	protected override void BeginPressEvent ()
	{
		transform.position = UICamera.lastHit.point;
	}

	protected override void UpdatePressEvent ()
	{
		float MH = root.manualHeight;
		float MW = Screen.width * MH / Screen.height;
		Vector3 lt = UICamera.lastTouchPosition;
		float ratio = MH / Screen.height;
		Vector3 lp = new Vector3 (lt.x - Screen.width / 2, lt.y - Screen.height / 2, 0) * ratio; 
		referToTra.transform.localPosition = lp;

		joystickTra.position = referToTra.position;
		if (Vector3.Distance (Vector3.zero, joystickTra.localPosition) >= MaxOffset)
		{
			joystickTra.localPosition = joystickTra.localPosition.normalized * MaxOffset;
		}
		m_postion = joystickTra.localPosition.normalized;
	}

	protected override void EndPressEvent ()
	{
		m_postion = Vector3.zero;
		joystickTra.localPosition = Vector3.zero;
		transform.localPosition = Vector3.zero;
	}
}
