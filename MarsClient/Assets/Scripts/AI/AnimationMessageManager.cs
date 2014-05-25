using UnityEngine;
using System.Collections;

public class AnimationMessageManager : MonoBehaviour {

	private AnimationController m_Animation;

	void Awake ()
	{
		m_Animation = transform.parent.GetComponent<AnimationController>();
	}

	void IdleMessage (int c)
	{
		m_Animation.IdleMessage (c);
	}

	void AttackMessage (int c)
	{
		m_Animation.AttackMessage (c);
	}

	void AttackAOEMessage (int c)
	{
		m_Animation.AttackAOEMessage (c);
	}

	void SpellAssault (int c)
	{
		m_Animation.SpellAssault (c);
	}
}
