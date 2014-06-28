using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour {
	
	
	private static Joystick instance;
	public static Vector2 getPosition
	{
		get{
			if (instance == null)
			{
				GameObject res_Go = Resources.Load ("Joystick", typeof (GameObject)) as GameObject;
				GameObject go = GameObject.Instantiate (res_Go) as GameObject;
				instance = go.GetComponentInChildren<Joystick> ();
				//instance = GameObject.FindObjectOfType(typeof (Joystick)) as Joystick;
			}
			return instance.position;
		}
	}
	
	private Vector2 min = Vector2.zero;
	private Vector2 max = Vector2.zero;
	
	static private Joystick[] joysticks;					// A static collection of all joysticks
	static private bool enumeratedJoysticks = false;
	static private float tapTimeDelta = 0.3f;				// Time allowed between taps
	
	public bool touchPad; 									// Is this a TouchPad?
	public Rect touchZone;
	public float deadZone = 0;									// Control when position is output
	public bool normalize = false; 							// Normalize output after the dead-zone?
	public Vector2 position; 									// [-1, 1] in x,y
	public int tapCount;											// Current tap count
	
	
	private int lastFingerId = -1;								// Finger last used for this joystick
	private float tapTimeWindow;							// How much time there is left for a tap to occur
	private Vector2 fingerDownPos;
	private float fingerDownTime;
	private float firstDeltaTime = 0.5f;
	
	private GUITexture gui;								// Joystick graphic
	private Rect defaultRect;								// Default position / extents of the joystick graphic
	// Boundary for joystick graphic
	private Vector2 guiTouchOffset;						// Offset to apply to touch input
	private Vector2 guiCenter;							// Center of joystick
	
	
	void Awake ()
	{
		//gameObject.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
		// Cache this component at startup instead of looking up every frame
		gui = GetComponent<GUITexture> ();
		
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;
		
		defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
		defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
		
		transform.position = new Vector3(0, 0, transform.position.z);
		
		if (touchPad) {
			// If a texture has been assigned, then use the rect ferom the gui as our touchZone
			if (gui.texture)
				touchZone = defaultRect;
		}
		else {
			// This is an offset for touch input to match with the top left
			// corner of the GUI
			guiTouchOffset.x = defaultRect.width * 0.5F;
			guiTouchOffset.y = defaultRect.height * 0.5F;
			
			// Cache the center of the GUI, since it doesn't change
			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;
			
			// Let's build the GUI boundary, so we can clamp joystick movement
			min.x = defaultRect.x - guiTouchOffset.x;
			max.x = defaultRect.x + guiTouchOffset.x;
			min.y = defaultRect.y - guiTouchOffset.y;
			max.y = defaultRect.y + guiTouchOffset.y;
		}
	}
	
	void Disable () {
		gameObject.SetActive (false);
		enumeratedJoysticks = false;
	}
	
	void ResetJoystick () {
		// Release the finger control and set the joystick back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
		
		if (touchPad){
			Color c = gui.color;
			c.a = 0.025f;
			gui.color = c;
		}
	}
	
	bool IsFingerDown () {
		return (lastFingerId != -1);
	}
	
	void LatchedFinger (int fingerId) {
		// If another joystick has latched this finger, then we must release it
		if (lastFingerId == fingerId)
			ResetJoystick ();
	}
	
	void Update () {
		if (!enumeratedJoysticks) {
			// Collect all joysticks in the game, so we can relay finger latching messages
			joysticks = FindObjectsOfType (typeof(Joystick)) as Joystick[];
			enumeratedJoysticks = true;
		}
		
		int count = Input.touchCount;
		
		// Adjust the tap time window while it still available
		if (tapTimeWindow > 0)
			tapTimeWindow -= Time.deltaTime;
		else
			tapCount = 0;
		
		if (count == 0) {
			ResetJoystick ();
		}
		else {
			for (int i = 0; i < count; i++) {
				Touch touch = Input.GetTouch (i);
				Vector2 guiTouchPos = touch.position - guiTouchOffset;
				
				bool shouldLatchFinger = false;
				if (touchPad) {
					if (touchZone.Contains (touch.position))
						shouldLatchFinger = true;
				}
				else if (gui.HitTest (touch.position)) {
					shouldLatchFinger = true;
				}
				
				// Latch the finger if this is a new touch
				if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)) {
					
					if (touchPad) {
						Color c = gui.color;
						c.a = 0.15f;
						gui.color = c;
						
						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
						fingerDownTime = Time.time;
					}
					
					lastFingerId = touch.fingerId;
					
					// Accumulate taps if it is within the time window
					if (tapTimeWindow > 0) {
						tapCount++;
					}
					else {
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					
					// Tell other joysticks we've latched this finger
					foreach (Joystick j in joysticks) {
						if (j != null && j != this)
						{
							j.LatchedFinger (touch.fingerId);
						}
					}
				}
				
				if (lastFingerId == touch.fingerId) {
					// Override the tap count with what the iPhone SDK reports if it is greater
					// This is a workaround, since the iPhone SDK does not currently track taps
					// for multiple touches
					if (touch.tapCount > tapCount)
						tapCount = touch.tapCount;
					
					if (touchPad) {
						// For a touchpad, let's just set the position directly based on distance from initial touchdown
						position.x = Mathf.Clamp ((touch.position.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1);
						position.y = Mathf.Clamp ((touch.position.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1);
						position = position.normalized;
					}
					else {
						// Change the location of the joystick graphic to match where the touch is
						position.x = (touch.position.x - guiCenter.x) / guiTouchOffset.x;
						position.y = (touch.position.y - guiCenter.y) / guiTouchOffset.y;
						position = position.normalized;
					}
					
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						position = Vector2.zero;
						ResetJoystick ();
					}
					else
					{
						//position = Vector2.one;
					}
				}
			}
		}
		
		// Calculate the length. This involves a squareroot operation,
		// so it's slightly expensive. We re-use this length for multiple
		// things below to avoid doing the square-root more than one.
		float length = position.magnitude;
		
		
		if (length < deadZone) {
			// If the length of the vector is smaller than the deadZone radius,
			// set the position to the origin.
			position = Vector2.zero;
		}
		else {
			if (length > 1) {
				// Normalize the vector if its length was greater than 1.
				// Use the already calculated length instead of using Normalize().
				position = position / length;
			}
			else if (normalize) {
				// Normalize the vector and multiply it with the length adjusted
				// to compensate for the deadZone radius.
				// This prevents the position from snapping from zero to the deadZone radius.
				position = position / length * Mathf.InverseLerp (length, deadZone, 1);
			}
		}
		
		if (!touchPad) {
			// Change the location of the joystick graphic to match the position
			float xx = (position.x - 1) * guiTouchOffset.x + guiCenter.x;
			float yy = (position.y - 1) * guiTouchOffset.y + guiCenter.y;
			gui.pixelInset = new Rect(xx, yy, gui.pixelInset.width, gui.pixelInset.height);
		}
	}
	
}
