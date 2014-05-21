using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public float active = 3.0f;

	void OnEnable ()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
		Invoke ("DisableGo", active);
	}

	void OnDisable ()
	{
		CancelInvoke ("DisableGo");
	}

	void DisableGo ()
	{
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.time);
	}
}
