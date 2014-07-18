using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	private float range = 0.5f;
	void Start ()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
		Vector3 pos = transform.position;
		transform.position = new Vector3 (Random.Range (pos.x -range, pos.x + range), 0.01f, Random.Range (pos.z -range, pos.z + range));
	}
	void Update ()
	{
		if(!particleSystem.IsAlive(true))
		{
			GameObject.Destroy(gameObject);
		}
	}
}
