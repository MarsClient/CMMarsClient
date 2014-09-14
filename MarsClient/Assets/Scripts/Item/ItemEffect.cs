using UnityEngine;
using System.Collections;

public class ItemEffect : MonoBehaviour {

	private const float PI = 3.1415926535898f;

	public float maxHeigh = 50;
	public float rateX = 0.05f;
	public float hiddenAngle = 120;
	public float spd = 10;
	public Transform particleSystem;

	private Transform mTransform;
	private Transform mParent;
	private float lastTime = 0;
	private float lastTweenTime = 0;
	private float dir; //1 or -1;
	private float dirValue
	{
		get
		{
			return Mathf.Abs (dir);
		}
	}
	private bool isOk = true;

	void OnEnable () 
	{
		mTransform = transform;
		lastTime = 0;
		lastTweenTime = Time.time;
		dir = 1;
		mParent = mTransform.parent;
		mParent.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
		isOk = false;
		particleSystem.gameObject.SetActive (false);
	}
	
	void Update () 
	{
		if (isOk == false)
		{
			mTransform.Rotate (new Vector3 (30, 1, 20) * Time.deltaTime * 50);
			float posY = mTransform.localPosition.y;
//			Debug.LogError (posY);
			lastTime += Time.deltaTime * spd;
			float x = lastTime * dir;
			float angle = lastTime * rateX;
			float y = Mathf.Sin (angle) * maxHeigh;
			mTransform.localPosition = new Vector3 (x, y, 0);
			float final = angle * dirValue;
			float maxAngle = 2 * PI * hiddenAngle / 360;
			
			float time = maxAngle / (spd * rateX);//(0-1)

			float a = 1 - (Time.time - lastTweenTime) / time;
			if (a <= 0)
			{
				Vector3 pos = mTransform.localPosition;
				pos.y = 0;
				mTransform.localPosition = pos;
				mTransform.rotation = Quaternion.Euler (Vector3.zero);
				particleSystem.localPosition = pos;
				particleSystem.gameObject.SetActive (true);

				isOk = true;
				//Destroy (mParent.gameObject, 2.0f);
			}
		}
	}
}
