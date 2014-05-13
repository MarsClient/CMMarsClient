using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Clip
{
	Idle,
	Run,
	Attack1,
	Attack2,
	Attack3,
}

[System.Serializable]
public class AnimationItem
{
	public Clip clip;
	public AnimationClip animationClip;
	public float speed = 1;
	public string clipName
	{
		get{
			return animationClip.name;
		}
	}

	public void SetSpeed (Animation ant)
	{
		ant[clipName].speed = speed;
	}
}
public class AnimationController : MonoBehaviour {

	public AnimationItem[] animationItems;
	private Dictionary<Clip, AnimationItem> antPools = new Dictionary<Clip, AnimationItem>();

	public Clip currentClip = Clip.Idle;

	public bool isAttack
	{
		get
		{
			return currentClip.ToString().Contains ("Attack");
		}
	}

	public bool doNotMove
	{
		get{
			return isAttack;
		}
	}

	public float length
	{
		get
		{
			return antPools[currentClip].animationClip.length / animation[currentClip.ToString()].speed;
		}
	}

	void Start ()
	{
		foreach (AnimationItem ai in animationItems)
		{
			ai.SetSpeed (animation);
			antPools.Add (ai.clip, ai);
		}
	}

	void Update ()
	{
		if (animation.isPlaying == false)
		{
			Play (Clip.Idle);
		}
	}

	public void Play (Clip clip)
	{
		if (currentClip != clip)
		{
			currentClip = clip;
			animation.CrossFade (antPools[clip].clipName);
		}
	}
}
