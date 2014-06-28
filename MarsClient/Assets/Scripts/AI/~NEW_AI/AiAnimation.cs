using UnityEngine;
using System;
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
	public const float ANIMATION_OFFSET = 0.05f;
}

[System.Serializable]
public class FrameEvent
{
	public int frame;
	public string method;
	public float antDisatnce; //>0 forward. <0 back. =0 dont move, when animationing.
	public float antMoveSpd; // about ant distance, mean that att or spell move spell
}
[System.Serializable]
public class AnimationInfo
{
	/*Write in Inspector*/
	public AnimationClip animationClip;//animation mode
	public Clip clip;//animation type
	public float speed = 1.0f;//if speed is zero, it will deafult one
	public string onCompleteCallback;//not loop animation for callback when animation end
	public List<FrameEvent> events;//which frame call something event (in fact send message)
	//public bool isAoeAllFrames = false;

	/*Some properties for get*/
	//
	[HideInInspector] public float length;//animationClip animation time.

	/*int method*/
	public void Init (Animation ant) 
	{
		SetSpeed (ant);
		OnCompleteEvent ();
		//AniamtionAllMesaage ();
		for (int i = 0; i < events.Count; i++) { SetEvent (events[i].method, events[i].frame / animationClip.frameRate, i); }
	}
	public void AniamtionAllMesaage ()
	{
//		if (this.isAoeAllFrames == true)
//		{
//			int frames = (int) (animationClip.frameRate * animationClip.length);
//			int startIndex = events.Count;
//			if (startIndex > 0)
//			{
//				FrameEvent frameEvent = events[startIndex - 1];
//				for (int i = startIndex - 1; i < frames; i++)
//				{
//					events.Add (frameEvent);
//				}
//			}
//		}
	}
	public void OnCompleteEvent () { if (animationClip.wrapMode == WrapMode.Default || animationClip.wrapMode == WrapMode.Once) { SetEvent (onCompleteCallback, animationClip.length - AntDefine.ANIMATION_OFFSET); } }
	private void SetEvent (string onEvent, float time, int index = -1)
	{
		if (time != 0 && onEvent != "")
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.functionName = onEvent;

			//Type c = this.GetType ();
			//object o = (object)this
			//UnityEngine.Object o = (UnityEngine.Object) this;
			string paramter = "";
			string _clip = ((int) clip).ToString ();
			paramter = _clip;
			if (index != -1)
			{
				string _index = index.ToString ();// (events.FindIndex (fe)).ToString ();
				paramter += "," + _index;
			}

			animationEvent.stringParameter = paramter;//JsonConvert.SerializeObject (fe);//(int) clip;

			animationEvent.messageOptions = SendMessageOptions.RequireReceiver;
			animationEvent.time = time;
			animationClip.AddEvent (animationEvent);
		}
	}
	private void SetSpeed (Animation ant) { if (speed == 0) { speed = 1; } length = animationClip.length / speed; ant[animationClip.name].speed = speed; }
	public FrameEvent getEvent (int index) { if (index >= 0 && index < events.Count) { return events[index]; } return null; }
}

[RequireComponent(typeof (TrailsManager))]
[RequireComponent(typeof (AiMove))]
public class AiAnimation : MonoBehaviour {

	/* Set Delegates attack spell or other function for call*/
	public delegate void AttackDelegate (AnimationInfo info, FrameEvent fe);
	public AttackDelegate attackDelegate;

	public AnimationInfo[] allAntInfos;//Write in Inspector
	private Dictionary<Clip, AnimationInfo> m_infos = new Dictionary<Clip, AnimationInfo> ();//all  animation infos;
	private Animation m_Animation;//
	private TrailsManager trailsManager;//weapon

	private AiMove _aiMove;
	public AiMove aiMove { get { if (_aiMove == null) { _aiMove = GetComponent <AiMove> (); } return _aiMove; } }
	[HideInInspector] public Transform m_Transform;

	[HideInInspector]
	public List<AnimationInfo> normalAttack = new List<AnimationInfo> ();

	private Clip clip = Clip.Idle;


	void Start () 
	{
		m_Animation = GetComponentInChildren<Animation>();
		m_Transform = transform;

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

	public void Play (Clip c)
	{
		if (clip != c)
		{
			AnimationInfo animationInfo = GetInfoByClip (c);
			bool isExist = animationInfo != null;
			if (isExist == true)
			{
				//Debug.Log (m_Animation.IsPlaying (animationInfo.animationClip.name) +  "___" + clip);
				//if (m_Animation.IsPlaying (animationInfo.animationClip.name)) { return; }
				clip = c;
				m_Animation.CrossFade (animationInfo.animationClip.name);
			}
		}
	}
	public void Stop ()
	{
		m_Animation.Stop ();
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
	public void IdleMessage ()
	{
		Play (Clip.Idle);
	}

	public void AttackMessage (int c, int eventIndex)
	{
		if (attackDelegate != null)
		{
			AnimationInfo info = GetInfoByClip ((Clip) c);
			attackDelegate (info, info.getEvent (eventIndex));
		}
	}

	public void AnimationMove (int c, int eventIndex)
	{
		AnimationInfo info = GetInfoByClip ((Clip) c);
		aiMove.startMoveDir (info, info.getEvent (eventIndex));
	}
}
