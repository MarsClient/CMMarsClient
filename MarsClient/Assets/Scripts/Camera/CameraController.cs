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
	public Transform target;
	public CameraType cameraType = CameraType.Null;
	//
	private Vector3 startPos;
	private float lastTime;
	private Vector3 lastPos;
	private int shakeIndex;

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
				CancelCameraShakeFunc ();
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
				CancelCameraShakeFunc ();
				transform.position = targetPos;
			}
			else if (cameraType == CameraType.Shake)
			{
				CameraShakeFunc ();
				//InvokeRepeating ("CameraShakeFunc", 0, 0);
			}
		}
	}

	void CameraShakeFunc ()
	{
		if ((shakeIndex++) % 2 == 0)
		{
			camera.fieldOfView = 29;
		}
		else
		{
			camera.fieldOfView = 23;
		}
	}

	void CancelCameraShakeFunc ()
	{
		shakeIndex = 0;
		camera.fieldOfView = 23;
	}
}
