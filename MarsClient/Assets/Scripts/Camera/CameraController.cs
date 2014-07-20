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
	private int shakeIndex;
	private float lastShakeTime;
	private float mShakeDuration;

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
				if (lastPos != target.position)
				{
					lastPos = target.position;
					//if (Time.time - lastTime >= 1)
					//{
					lastTime = Time.time;
					//}
				}
				transform.position = Vector3.Lerp (transform.position, targetPos, Time.time - lastTime);
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
}
