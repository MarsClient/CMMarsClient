﻿using UnityEngine;
using System.Collections;

public interface IAnimationListener
{
	void IdleMessage (string info);
	void AttackMessage (string info);
	void AnimationMove (string info);
	void AnimationFx (string info);
	void AnimationShake (string info);
	void AnimationSpellAttack (string info);
	void DeathDoneMessage (string info);
}

public class AnimationMessageManager : MonoBehaviour, IAnimationListener {

	private AiAnimation aiAnimation;

	void Awake ()
	{
		aiAnimation = transform.parent.GetComponent<AiAnimation>();
	}

	public int[] SetAnimationIdex (string info)
	{
		string[] infos = info.Split (',');
		int c = int.Parse (infos[0]);
		int eventIndex = int.Parse (infos[1]);
		return new int[2] { c, eventIndex };
	}

	#region IAnimationListener implementation
	public void IdleMessage (string info)
	{
		aiAnimation.IdleMessage ();
	}

	public void AttackMessage (string info)
	{
		int[] events = SetAnimationIdex (info);
		aiAnimation.AttackMessage (events[0], events[1]);
	}

	public void AnimationMove (string info)
	{
		int[] events = SetAnimationIdex (info);
		aiAnimation.AnimationMove (events[0], events[1]);
	}

	public void AnimationFx (string info)
	{
		int[] events = SetAnimationIdex (info);
		aiAnimation.AnimationFx (events[0], events[1]);
	}

	public void AnimationShake (string info)
	{
		int[] events = SetAnimationIdex (info);
		aiAnimation.AnimationShake (events[0], events[1]);
	}

	public void AnimationSpellAttack (string info)
	{
		int[] events = SetAnimationIdex (info);
		aiAnimation.AnimationSpellAttack (events[0], events[1]);
	}

	public void DeathDoneMessage (string info)
	{
		aiAnimation.AnimationDeath ();
	}
	#endregion
}
