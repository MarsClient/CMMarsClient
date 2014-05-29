using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Clip : int
{
	Null,
	Idle,
	Run,
	Attack1,
	Attack2,
	Attack3,
	Hit,
	Fall,
	Die,
	Spell1,
	Spell2,
}

public class AntDefine
{
	public const string KEY_ATTACK = "Attack";
	public const string KEY_SPELL = "Spell";
}

[System.Serializable]
public class FrameEvent
{
	public int frame;
	public string method;
}
[System.Serializable]
public class AnimationInfo
{
	/*Write in Inspector*/
	public AnimationClip animationClip;
	public Clip clip;
	public float speed = 1.0f;//if speed is zero, it will defu
	public string onCompleteCallback;
	public FrameEvent[] events;

	/*Some properties for get*/
	//[HideInInspector]
	public float length;

	/*int method*/
	public void Init (Animation ant) 
	{
		SetSpeed (ant);
		OnCompleteEvent ();
		foreach (FrameEvent fe in events) { SetEvent (fe.method, fe.frame / animationClip.frameRate); }
	}
	public void OnCompleteEvent () { if (animationClip.wrapMode == WrapMode.Default || animationClip.wrapMode == WrapMode.Once) { SetEvent (onCompleteCallback, animationClip.length - 0.05f); } }
	private void SetEvent (string onEvent, float time)
	{
		if (time != 0 && onEvent != "")
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.functionName = onEvent;
			animationEvent.intParameter = (int) clip;
			animationEvent.messageOptions = SendMessageOptions.RequireReceiver;
			animationEvent.time = time;
			animationClip.AddEvent (animationEvent);
		}
	}
	private void SetSpeed (Animation ant) { if (speed == 0) { speed = 1; } length = animationClip.length / speed; ant[animationClip.name].speed = speed; }
}

[RequireComponent(typeof (TrailsManager))]
public class AiAnimation : MonoBehaviour {

	public AnimationInfo[] allAntInfos;
	private Dictionary<Clip, AnimationInfo> m_infos = new Dictionary<Clip, AnimationInfo> ();
	private Animation m_Animation;
	private TrailsManager trailsManager;

	[HideInInspector]
	public List<AnimationInfo> normalAttack = new List<AnimationInfo> ();

	void Start () 
	{
		m_Animation = GetComponentInChildren<Animation>();
		trailsManager = GetComponentInChildren<TrailsManager>();
		foreach (AnimationInfo ai in allAntInfos) 
		{
			ai.Init (m_Animation);
			m_infos.Add (ai.clip, ai);
			if (isContainState (ai.clip, AntDefine.KEY_ATTACK))
			{
				normalAttack.Add (ai);
			}
		}
	}

	private Clip clip = Clip.Idle;
	public void Play (Clip c)
	{
		if (clip != c)
		{
			AnimationInfo animationInfo = GetInfoByClip (c);
			bool isExist = animationInfo != null;
			if (isExist == true)
			{
				//Debug.Log (m_Animation.IsPlaying (animationInfo.animationClip.name) +  "___" + clip);
				if (m_Animation.IsPlaying (animationInfo.animationClip.name)) { return; }
				clip = c;
				m_Animation.CrossFade (animationInfo.animationClip.name);
			}
		}
	}

	/*current state*/
	public bool isAttack { get { return isContainState (clip, AntDefine.KEY_ATTACK); } }
	public bool isSpell { get { return isContainState (clip, AntDefine.KEY_SPELL); } }
	public bool isFall { get { return clip == Clip.Fall; } }
	public bool isHitted { get { return clip == Clip.Hit; } }
	public bool dontMove { get { return isAttack || isSpell || isFall || isHitted; } }

	/*about info*/
	bool isContainState (Clip clip, string key)
	{
		return clip.ToString ().Contains (key);
	}

	public AnimationInfo GetInfoByClip (Clip c)
	{
		AnimationInfo animationInfo = null;
		m_infos.TryGetValue (c, out animationInfo);
		return animationInfo;
	}

	/*This is weapon light effect*/
	void LateUpdate ()
	{
		trailsManager.RunAnimations (m_Animation, isAttack || isSpell);
	}

	/*Follow is Animation message receive*/
	public void IdleMessage (int c)
	{
		Play (Clip.Idle);
	}
}
