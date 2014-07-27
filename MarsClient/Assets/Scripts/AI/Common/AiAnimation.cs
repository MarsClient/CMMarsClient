using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Clip : int
{
	Null = 0,
	Idle,
	Run,
	Attack1 = 3,
	Attack2,
	Attack3,
	Hit,
	Fall,
	Die,
	Spell1 = 9,
	Spell2 = 10,
	Spell3 = 11,
}

public class AntDefine
{
	public const string KEY_RUN = "Run";
	public const string KEY_IDLE = "Idle";
	public const string KEY_ATTACK = "Attack";
	public const string KEY_SPELL = "Spell";
	//public const float ANIMATION_OFFSET = 0.05f;
}

[System.Serializable]
public class FrameEvent
{
	public int frame;
	public string method;
	public bool isLoopCallback;//>=frame, will update all in the animation Clip
	public float updateRate;
	public float shakeTime;
	public float antDisatnce; //>0 forward. <0 back. =0 dont move, when animationing.
	public float antMoveSpd; // about ant distance, mean that att or spell move spell
	public Clip attackedClip = Clip.Idle;// attacked state
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
	}
	private void SetSpeed (Animation ant) { if (speed == 0) { speed = 1; } length = animationClip.length / speed; ant[animationClip.name].speed = speed; }
	public FrameEvent getEvent (int index) { if (index >= 0 && index < events.Count) { return events[index]; } return null; }
}

[RequireComponent(typeof (TrailsManager))]
//[RequireComponent(typeof (AiMove))]
public class AiAnimation : MonoBehaviour {

	/* Set Delegates attack or other function for call*/
	public delegate void AttackDelegate (AnimationInfo info, FrameEvent fe);
	public AttackDelegate attackDelegate;

	/* Set Delegates spell or other function for call*/
	public delegate void SpellAttackDelegate (AnimationInfo info, FrameEvent fe);
	public SpellAttackDelegate spellAttackDelegate;

	/* Unit Death*/
	public delegate void UnitDeath ();
	public UnitDeath unitDeath;

	/* Fx */
	public delegate void FxDelegate (AnimationInfo info, FrameEvent fe);
	public FxDelegate fxDelegate;

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
	private AnimationInfo m_AnimationInfo;

	private float beginPlayTime = 0;//start paly time
	private float prefabPlayTime = 0;//prefab play time
	private float lastUpdateAnimationMessageTime = 0;

	void Start () 
	{
		m_Animation = GetComponentInChildren<Animation>();
		if (m_Animation.GetComponent<AnimationMessageManager>() == null)
		{
			m_Animation.gameObject.AddComponent <AnimationMessageManager>();
		}
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
		if (isDie) { return; }
		if (clip != c)
		{
			m_AnimationInfo = GetInfoByClip (c);
			bool isExist = m_AnimationInfo != null;
			if (isExist == true)
			{
				clip = c;
				m_Animation.CrossFade (m_AnimationInfo.animationClip.name);
				beginPlayTime = Time.time;
			}
		}
	}
	public void Stop ()
	{
		m_Animation.Stop ();
	}

	/*current state*/
	public bool isIdle { get { return isContainState (clip, AntDefine.KEY_IDLE); } }
	public bool isRun { get { return isContainState (clip, AntDefine.KEY_RUN); } }
	public bool isAttack { get { return isContainState (clip, AntDefine.KEY_ATTACK); } }
	public bool isSpell { get { return isContainState (clip, AntDefine.KEY_SPELL); } }
	public bool isFall { get { return clip == Clip.Fall; } }
	public bool isHitted { get { return clip == Clip.Hit; } }
	public bool isDie { get { return clip == Clip.Die; } }
	public bool dontMove { get { return isAttack || isSpell || isFall || isHitted || isDie; } }

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

	/*@effect: This is weapon light effect*/
	/*@frameEffect: all frameEvents in one animationClip that will be called*/
	void LateUpdate ()
	{
		trailsManager.RunAnimations (m_Animation, isAttack || isSpell);

		if (m_AnimationInfo != null && m_AnimationInfo.animationClip.wrapMode != WrapMode.Loop)
		{
			float m_tt = (Time.time - beginPlayTime) * m_AnimationInfo.speed;

			if (m_AnimationInfo.events.Count > 0)
			{
				for (int m_i = 0; m_i < m_AnimationInfo.events.Count; m_i++)
				{
					FrameEvent frameEvent = m_AnimationInfo.events[m_i];
					float frame = frameEvent.frame / m_AnimationInfo.animationClip.frameRate;
					if (frame > prefabPlayTime && frame <= m_tt && frameEvent.isLoopCallback == false)
					{
						m_Animation.SendMessage (frameEvent.method, ((int)m_AnimationInfo.clip).ToString () + "," + m_i.ToString (), SendMessageOptions.RequireReceiver);
					}
					else if (frame <= m_tt && frameEvent.isLoopCallback == true)
					{
						if (Time.time - lastUpdateAnimationMessageTime > frameEvent.updateRate)
						{
							lastUpdateAnimationMessageTime = Time.time;
							m_Animation.SendMessage (frameEvent.method, ((int)m_AnimationInfo.clip).ToString () + "," + m_i.ToString (), SendMessageOptions.RequireReceiver);
						}
					}
				}
			}

			if (m_tt >= m_AnimationInfo.animationClip.length - 0.05f)
			{
				m_Animation.SendMessage (m_AnimationInfo.onCompleteCallback, m_AnimationInfo.clip.ToString (), SendMessageOptions.RequireReceiver);
			}
			prefabPlayTime = m_tt;
		}
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

	public void AnimationFx (int c, int eventIndex)
	{
		if (fxDelegate != null)
		{
			AnimationInfo info = GetInfoByClip ((Clip) c);
			fxDelegate (info, info.getEvent (eventIndex));
		}

	}

	public void AnimationShake (int c, int eventIndex)
	{
		AnimationInfo info = GetInfoByClip ((Clip) c);
		CameraController.instance.StartShake (info.getEvent (eventIndex).shakeTime, ()=>
		                                      {
			return CameraType.Follow;
		});
	}

	public void AnimationSpellAttack (int c, int eventIndex)
	{
		if (spellAttackDelegate != null)
		{
			AnimationInfo info = GetInfoByClip ((Clip) c);
			spellAttackDelegate (info, info.getEvent (eventIndex));
		}
	}

	public void AnimationDeath ()
	{
		if (unitDeath != null)
		{
			unitDeath ();
		}
	}
}
