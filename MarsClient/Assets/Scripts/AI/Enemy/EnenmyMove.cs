using UnityEngine;
using System.Collections;

public class EnenmyMove : MonoBehaviour {


	private float startTime;
	private float lastTime;
	private Vector3 targetPos;
	public void SetMove (float time, Vector3 targetPos)
	{
		startTime = Time.time;
		lastTime = time;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Time.time - startTime > lastTime)
		{

		}
	}
}
