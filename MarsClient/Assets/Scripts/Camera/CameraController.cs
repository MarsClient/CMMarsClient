using UnityEngine;
using System.Collections;

public enum CameraType
{
	Null,
	Follow,
	Shake,
	Move,
}

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	public delegate CameraType ShakeCompleteEvent ();
	private ShakeCompleteEvent shakeComplete;

	public Transform target;
	public CameraType cameraType = CameraType.Null;
	//
	private Vector3 startPos;
	private float lastTime;
	private Vector3 lastPos;
	private int shakeIndex;//every frame shake
	private float lastShakeTime;//remember last shake time 
	private float mShakeDuration;//shake Duration 

	private float followSpd = 1;
	private float spd = 1;

	void Awake ()
	{
		instance = this;
		startPos = transform.position - Vector3.zero;
		if (target != null)
		{
			initialize (target, CameraType.Follow);
		}
	}

	public void initialize (Transform target, CameraType type)
	{
		this.target = target;
		this.cameraType = type;
	}

	void LateUpdate ()
	{
		if (target == null)
		{
			return;
		}
		float x = target.position.x;
		float z = target.position.z;


		if (cameraType == CameraType.Null) {}
		else
		{
			Vector3 targetPos = startPos + target.position;
			if (cameraType == CameraType.Move)
			{
				ShakeComplete ();
				CheckDistance ();
				float currentPosX = transform.position.x;
				float currentPosZ = transform.position.z;
				float wantPosX = targetPos.x;
				float wantPosZ = targetPos.z;
				float xVal = Mathf.Lerp (currentPosX, wantPosX, followSpd * Time.deltaTime);
				float zVal = Mathf.Lerp (currentPosZ, wantPosZ, followSpd * Time.deltaTime);
				transform.position = new Vector3 (xVal, transform.position.y, zVal);

			}
			else if (cameraType == CameraType.Follow)
			{
				ShakeComplete ();
				transform.position = targetPos;
			}
			else if (cameraType == CameraType.Shake)
			{
				if (Time.time - lastShakeTime < mShakeDuration )
				{
					if ((shakeIndex++) % 2 == 0)
					{
						camera.fieldOfView = 25;
					}
					else
					{
						camera.fieldOfView = 23;
					}
					return;
				}
				if (shakeComplete != null)
				{
					cameraType = shakeComplete ();
					shakeComplete = null;
				}
			}
		}
	}

	#region SHAKE
	public void StartShake (float shakeDuration, ShakeCompleteEvent m_shakeComplete)
	{
		//Debug.LogError (shakeDuration);
		this.mShakeDuration = shakeDuration;
		lastShakeTime = Time.time;
		cameraType = CameraType.Shake;
		this.shakeComplete = m_shakeComplete;
	}

	void ShakeComplete ()
	{

		shakeIndex = 0;
		camera.fieldOfView = 23;
	}
	#endregion

	#region MOVE
	private void CheckDistance ()
	{
		Vector3 a = transform.position;
		a.y = 0;

		Vector3 b = target.position + startPos;
		b.y = 0;
		if (Vector3.Distance (a, b) >= 3)
		{
			followSpd += Time.deltaTime;
		}
		if (followSpd <= 1)
		{
			if (followSpd >= 1)
			{
				followSpd -= Time.deltaTime;
			}
		}
	}

	public void StartMove ()
	{
		cameraType = CameraType.Move;
	}
	#endregion
}
