using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public float active = 3.0f;

	private PoolController m_Pc;

	void OnEnable ()
	{
		//transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
		Invoke ("DisableGo", active);
	}

	void OnDisable ()
	{
		CancelInvoke ("DisableGo");
	}

	void DisableGo ()
	{
		if (m_Pc == null)
		{
			m_Pc = GetComponent<PoolController> ();
		}
		m_Pc.Release ();
	}
}
