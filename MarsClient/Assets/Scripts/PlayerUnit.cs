using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUnit : MonoBehaviour {

	public static List<PlayerUnit> playersUnit = new List<PlayerUnit> ();

	private AnimationController m_ac;
	public AnimationController ac { get{ return m_ac; } }

	public Transform hitPos;

	void Awake ()
	{
		m_ac = GetComponent <AnimationController>();
		playersUnit.Add (this);
	}

	void Remove ()
	{
		playersUnit.Remove (this);
	}

	public void Hitted (AnimationItem animationItem)
	{
		if (ac.isFall == false)
		{
			ac.Play (animationItem.targetClip);
		}
		ObjectPool.Instance.LoadObject ("EF/EF0001", hitPos.position);

		//hit color
		CancelInvoke ("ResetColor");
		SetColor (0.8f);
		Invoke ("ResetColor", 0.1f);
	}

	void ResetColor ()
	{
		SetColor (0);
	}
	
	void SetColor (float a)
	{
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
				Color c = m.color;
				c.a = a;
				m.color = c;
			}
		}
	}
}
