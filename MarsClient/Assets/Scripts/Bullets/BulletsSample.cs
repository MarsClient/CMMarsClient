using UnityEngine;
using System.Collections;

public class BulletsSample : MonoBehaviour {

	public delegate void AddDamage (HitUnit hu);
	public AddDamage m_AddDamage;


	public float moveSpeed = 10;

	public GameObject mainEffect;
	public GameObject hitEffect;


	private PoolController m_pc;
	private bool m_AllowUpdate = false;
	private Vector3 m_dir;
	//private bool OverlapSphere = true;
	public void InitBullets (AddDamage addDamage)
	{
		InitBullets (addDamage, Vector3.forward);
	}
	public void InitBullets (AddDamage addDamage, Vector3 dir) 
	{
		gameObject.SetActive (true);
		mainEffect.SetActive (true);
		if (hitEffect != null)
		{
			hitEffect.SetActive (false);
		}
		if (addDamage != null)
		{
			this.m_AddDamage = addDamage;
		}
		m_dir = dir;
		m_AllowUpdate = true;

		CancelInvoke ("HiddemEf");
		Invoke ("HiddemEf", 3);
	}

	void Update () 
	{
		if (m_AllowUpdate)
		{
			transform.Translate (m_dir * moveSpeed * Time.deltaTime);
			ExplosionDamage (mainEffect.transform.position, 0.5f);
		}

	}

	void ExplosionDamage(Vector3 center, float radius) 
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		if (hitColliders != null && hitColliders.Length > 0)
		{
			for (int i = 0; i < hitColliders.Length; i++)
			{
				HitUnit hu = hitColliders[i].GetComponent<HitUnit>();
				if (hu != null)
				{
					if (m_AddDamage != null) 
					{
						m_AddDamage (hu);
					}
				}
			}
			m_AllowUpdate = false;
			mainEffect.SetActive (false);
			if (hitEffect!= null)
			{
				hitEffect.SetActive (true);
			}

			CancelInvoke ("HiddemEf");
			Invoke ("HiddemEf", 1);
		}
	}

	void HiddemEf ()
	{
		if (m_pc == null) m_pc = GetComponent <PoolController>();
		m_pc.Release ();
		//gameObject.SetActive (false);
	}
}
