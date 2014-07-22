using UnityEngine;
using System.Collections;

public class BulletsSample : MonoBehaviour {

	public delegate void AddDamage (HitUnit hu);
	public AddDamage m_AddDamage;


	public float moveSpeed = 10;
	public float radius = 0.5f;
	public GameObject mainEffect;
	public GameObject hitEffect;
	public bool isNeedDisapear = true;
	public float hiddenTime = 3;


	private PoolController m_pc;
	private bool m_AllowUpdate = false;
	private Vector3 m_dir;
	//private bool OverlapSphere = true;
	public void InitBullets (AddDamage addDamage)
	{
		InitBullets (addDamage, true, Vector3.forward);
	}

	public void InitBullets (AddDamage addDamage, bool allowUpdate)
	{
		InitBullets (addDamage, allowUpdate, Vector3.zero);
	}

	public void InitBullets (AddDamage addDamage, bool allowUpdate, Vector3 dir) 
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
		m_AllowUpdate = allowUpdate;

		CancelInvoke ("HiddemEf");
		Invoke ("HiddemEf", hiddenTime);
	}


	public void InitPosition (Vector3 p)
	{
		transform.position = p;
	}

	void Update () 
	{
		if (m_AllowUpdate)
		{
			transform.Translate (m_dir * moveSpeed * Time.deltaTime);
			ExplosionDamage (mainEffect.transform.position, radius);
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
			if (isNeedDisapear == true)
			{
				mainEffect.SetActive (false);
				if (hitEffect!= null)
				{
					hitEffect.SetActive (true);
				}

				CancelInvoke ("HiddemEf");
				Invoke ("HiddemEf", 1);
			}
		}
	}

	void HiddemEf ()
	{
		if (m_pc == null) m_pc = GetComponent <PoolController>();
		m_pc.Release ();
		//gameObject.SetActive (false);
	}
}
