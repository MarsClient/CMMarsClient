using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailsManager : MonoBehaviour 
{
	private List<WeaponTrail> trails = new List<WeaponTrail>();

	/*
	 *New WeaponTrail
	 *
	 */
	protected float t = 0.033f;
	private float tempT = 0;
	protected float m = 0;
	protected Vector3 lastEulerAngles = Vector3.zero;
	protected Vector3 lastPosition = Vector3.zero;
	protected Vector3 eulerAngles = Vector3.zero;
	protected Vector3 position = Vector3.zero;
	protected float animationIncrement = 0.003f; // ** This sets the number of time the controller samples the animation for the weapon trails


	void Start ()
	{
		Init ();
	}

	void Init ()
	{
		foreach (WeaponTrail trail in GetComponentsInChildren<WeaponTrail> ())
		{
			trails.Add (trail);
			trail.StartTrail (.2f, .5f);
		}
	}

	public void RunAnimations (Animation m_Animation, bool active)
	{
		if (m_Animation != null && trails != null && trails.Count > 0)
		{
			for (int j = 0; j < trails.Count; j++) 
			{
				trails[j].hideMeshRenderActive (active);
			}
			
			if (t > 0)
			{
				eulerAngles = transform.eulerAngles;
				position = transform.localPosition;
				
				while (tempT < t) 
				{
					tempT += animationIncrement;
					m = tempT / t;
					transform.eulerAngles = new Vector3(Mathf.LerpAngle(lastEulerAngles.x, eulerAngles.x, m),Mathf.LerpAngle(lastEulerAngles.y, eulerAngles.y, m),Mathf.LerpAngle(lastEulerAngles.z, eulerAngles.z, m));
					transform.position = Vector3.Lerp(lastPosition, position, m);
					m_Animation.Sample ();
					for (int j = 0; j < trails.Count; j++) {
						if (trails[j].time > 0) {
							trails[j].Itterate (Time.time - t + tempT);
						} else {
							trails[j].ClearTrail ();
						}
					}
				}
				//
				// ** End of loop
				//
				tempT -= t;
				//
				// ** Sets the position and rotation to what they were originally
				transform.localPosition = position;
				transform.eulerAngles = eulerAngles;
				lastPosition = position;
				lastEulerAngles = eulerAngles;
				//
				// ** Finally creates the meshes for the WeaponTrails (one per frame)
				//
				for (int j = 0; j < trails.Count; j++) {
					if (trails[j].time > 0) {
						trails[j].UpdateTrail (Time.time, t);
					}
				}
			}
		}
	}
}
