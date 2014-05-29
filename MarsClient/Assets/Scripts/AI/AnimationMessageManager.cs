using UnityEngine;
using System.Collections;

public class AnimationMessageManager : MonoBehaviour {

	private AiAnimation aiAnimation;

	void Awake ()
	{
		aiAnimation = transform.parent.GetComponent<AiAnimation>();
	}

	void IdleMessage (int c)
	{
		aiAnimation.IdleMessage (c);
	}

	/*void AttackMessage (int c)
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
	}*/
}
