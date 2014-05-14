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

public class AnimationMessage
{
	public static string IDLE_MESSAGE = "IdleMessage";
	public static string ATTACK_MESSAGE = "AttackMessage";

}

[System.Serializable]
public class AnimationItem
{
	public Clip clip;
	public AnimationClip animationClip;
	public float speed = 1;
	public float[] eventTimes;
	public float actionMove = 0;
	public GameObject effect;
	public string clipName
	{
		get{
			return animationClip.name;
		}
	}

	public void Init (Animation ant)
	{
		SetSpeed (ant);
		AddCompleteEvent ();
		foreach (float t in eventTimes)
		{
			SetEvent (AnimationMessage.ATTACK_MESSAGE, t);
		}
	}

	private void AddCompleteEvent ()
	{
		if (animationClip.wrapMode == WrapMode.Default)
		{
			SetEvent (AnimationMessage.IDLE_MESSAGE);
		}
	}
	private void SetEvent (string onEvent)
	{
		SetEvent (onEvent, animationClip.length - 0.05f);
	}
	private void SetEvent (string onEvent, float time)
	{
		if (time == 0)
		{
			return;
		}
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.functionName = onEvent;
		animationEvent.messageOptions = SendMessageOptions.DontRequireReceiver;
		animationEvent.time = time;
		animationClip.AddEvent (animationEvent);
	}
	
	private void SetSpeed (Animation ant)
	{
		ant[clipName].speed = speed;
	}
}
public class AnimationController : MonoBehaviour {

	public delegate void AttackEvent (AnimationItem animationItem);
	public AttackEvent attackEvent;


	public AnimationItem[] animationItems;
	private Dictionary<Clip, AnimationItem> antPools = new Dictionary<Clip, AnimationItem>();

	public AnimationItem currentAnimationItem;

	public bool isAttack
	{
		get
		{
			return currentAnimationItem.clip.ToString().Contains ("Attack");
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
			ai.Init (animation);
			antPools.Add (ai.clip, ai);
		}
		currentAnimationItem = antPools[Clip.Idle];
	}

	private Clip currentClip;
	public void Play (Clip clip)
	{
		if (currentClip != clip)
		{
			currentClip = clip;
			currentAnimationItem = antPools[clip];
			animation.CrossFade (currentAnimationItem.clipName);
		}
	}

	void IdleMessage ()
	{
		Play (Clip.Idle);
	}

	void AttackMessage ()
	{
		Debug.Log (currentAnimationItem.clipName);
		if (attackEvent != null)
		{
			attackEvent (currentAnimationItem);
		}
	}
}
