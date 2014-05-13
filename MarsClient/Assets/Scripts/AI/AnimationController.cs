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
	Null,
}

[System.Serializable]
public class AnimationItem
{
	public Clip clip;
	public AnimationClip animationClip;
	public float speed = 1;
	public string onCompelte;
	public string clipName
	{
		get{
			return animationClip.name;
		}
	}

	public void AddEvent ()
	{
		if (animationClip.wrapMode == WrapMode.Default)
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.functionName = "Play";
			animationEvent.stringParameter = "Idle";
			animationEvent.messageOptions = SendMessageOptions.DontRequireReceiver;
			animationEvent.time = animationClip.length-0.1f;
			animationClip.AddEvent (animationEvent);
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

	public float GetLength (Clip clip)
	{
		return antPools[clip].animationClip.length / animation[clip.ToString()].speed;
	}

	void Start ()
	{
		foreach (AnimationItem ai in animationItems)
		{
			ai.SetSpeed (animation);
			ai.AddEvent ();
			antPools.Add (ai.clip, ai);
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
