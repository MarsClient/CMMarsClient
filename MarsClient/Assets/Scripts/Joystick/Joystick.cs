using UnityEngine;
using System.Collections;

public enum JoyStickType
{
	MoveJS,
	StaticJS,
}

public class JoyStick : MonoBehaviour {
	
	private static bool isFreeze = true;
	public static void SetFreeze (bool m_IsFreeze)
	{
		isFreeze = m_IsFreeze;
	}
	public JoyStickType JS_type = JoyStickType.MoveJS;
	
	public float MaxOffset = 50;
	public Transform joystickTra;
	public Camera m_camera;
	public LayerMask mask;
	
	private Vector2 m_postion;
	private UIRoot root;
	private Transform referToTra;
	private RaycastHit lastHit;
	
	const int MAXFIGHTID = -1;
	private int lastFingerId;
	public bool isTouching = false;
	
	void Awake ()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		gameObject.SetActive (false);
		return;
		#endif
		lastFingerId = MAXFIGHTID;
		root = NGUITools.FindInParents<UIRoot> (gameObject);
		GameObject go = new GameObject("referToTra");
		referToTra = go.transform;
		referToTra.parent = root.transform;
		referToTra.localScale = Vector3.one;
		referToTra.localPosition = Vector3.zero;
	}
	
	void Update ()
	{
		if (isFreeze)
		{
			Apply ();
		}
	}
	
	void Apply ()
	{
		int count = Input.touchCount;
		if (count <= 0)
		{
			EndPressEvent ();
		}
		for (int i = 0; i < count; i++)
		{
			Touch touch = Input.GetTouch (i);
			TouchPhase phase = touch.phase;
			if (phase == TouchPhase.Began)
			{
				if (this.GetColliderActive () == true)
				{
					if (isTouchZone (touch) && (lastFingerId == MAXFIGHTID || lastFingerId != touch.fingerId))
					{
						SetCollider (false);
						lastFingerId = touch.fingerId;
						BeginPressEvent ();
					}
					
				}
			}
			if (lastFingerId == touch.fingerId)
			{
				if (phase == TouchPhase.Moved)
				{
					UpdatePressEvent (touch);
				}
				else if (phase == TouchPhase.Ended)
				{
					EndPressEvent ();
				}
			}
		}
	}
	
	private bool isTouchZone (Touch touch)
	{
		Ray ray = m_camera.ScreenPointToRay  (touch.position);
		if (Physics.Raycast(ray, out lastHit, Mathf.Infinity, mask) == false)
		{
			return false;
		}
		
		Collider c = lastHit.collider;
		return  this.collider == c;
	}
	
	private void SetCollider (bool isBy)
	{
		if (collider.enabled != isBy)
		{
			collider.enabled = isBy;
		}
	}
	
	private bool GetColliderActive ()
	{
		return collider.enabled;
	}
	
	protected void BeginPressEvent ()
	{
		if (JS_type == JoyStickType.MoveJS)
		{
			transform.position = lastHit.point;
		}
		else
		{
			
		}
	}
	
	protected void UpdatePressEvent (Touch touch)
	{
		if (root == null)
		{
			Debug.LogError ("No root reference");
			return;
		}
		float MH = root.manualHeight;
		float MW = Screen.width * MH / Screen.height;
		Vector3 lt = touch.position;
		float ratio = MH / Screen.height;
		Vector3 lp = new Vector3 (lt.x - Screen.width / 2, lt.y - Screen.height / 2, 0) * ratio; 
		referToTra.transform.localPosition = lp;
		
		joystickTra.position = referToTra.position;
		if (Vector3.Distance (Vector3.zero, joystickTra.localPosition) >= MaxOffset)
		{
			joystickTra.localPosition = joystickTra.localPosition.normalized * MaxOffset;
		}
		m_postion = joystickTra.localPosition.normalized;

		//Start move
		AiUpdateMove ();
	}
	
	protected void EndPressEvent ()
	{
		SetCollider (true);
		lastFingerId = MAXFIGHTID;
		m_postion = Vector3.zero;
		joystickTra.localPosition = Vector3.zero;
		transform.localPosition = Vector3.zero;
		
		//Start move
		AiUpdateMove ();
	}
	
	private void AiUpdateMove ()
	{
		float x = -m_postion.x;
		float z = -m_postion.y;
		AiInput.instance.UpdateMove (new Vector3 (x, 0, z));
	}
}