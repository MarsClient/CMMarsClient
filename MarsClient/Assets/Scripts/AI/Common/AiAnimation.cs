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
	public const float ANIMATION_OFFSET = 0.01f;
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
	public float frameRate { get { return animationClip.frameRate * speed; } }

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
				if (m_AnimationInfo.animationClip.wrapMode != WrapMode.Loop)
				{
					beginPlayTime = Time.time;
					Stop ();

				}
				//Debug.Log (beginPlayTime + "_________" + c);
				m_Animation.CrossFade (m_AnimationInfo.animationClip.name);
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
		//trailsManager.RunAnimations (m_Animation, isAttack || isSpell);

		if (m_AnimationInfo != null && m_AnimationInfo.animationClip.wrapMode != WrapMode.Loop)
		{
			float m_tt = Time.time - beginPlayTime;//go through time; length * spd
			if (m_AnimationInfo.events.Count > 0)
			{
				for (int m_i = 0; m_i < m_AnimationInfo.events.Count; m_i++)
				{
					FrameEvent frameEvent = m_AnimationInfo.events[m_i];
					float frame = frameEvent.frame / (m_AnimationInfo.frameRate );
					if (frame > prefabPlayTime && frame <= m_tt && frameEvent.isLoopCallback == false)
					{
						gameObject.SendMessage (frameEvent.method, ((int)m_AnimationInfo.clip).ToString () + "," + m_i.ToString (), SendMessageOptions.RequireReceiver);
					}
					else if (frame <= m_tt && frameEvent.isLoopCallback == true)
					{
						if (Time.time - lastUpdateAnimationMessageTime > frameEvent.updateRate)
						{
							lastUpdateAnimationMessageTime = Time.time;
							gameObject.SendMessage (frameEvent.method, ((int)m_AnimationInfo.clip).ToString () + "," + m_i.ToString (), SendMessageOptions.RequireReceiver);
						}
					}
				}
			}

			float length = m_AnimationInfo.length;
			if (m_tt >= length)
			{
				gameObject.SendMessage (m_AnimationInfo.onCompleteCallback, m_AnimationInfo.clip.ToString (), SendMessageOptions.RequireReceiver);
			}
			prefabPlayTime = m_tt;
		}
	}

	/*Follow is Animation message receive*/
	public void IdleMessageCall ()
	{
		Play (Clip.Idle);
	}

	public void AttackMessageCall (int c, int eventIndex)
	{
		if (attackDelegate != null)
		{
			AnimationInfo info = GetInfoByClip ((Clip) c);
			attackDelegate (info, info.getEvent (eventIndex));
		}
	}

	public void AnimationMoveCall (int c, int eventIndex)
	{
		AnimationInfo info = GetInfoByClip ((Clip) c);
		aiMove.startMoveDir (info, info.getEvent (eventIndex));
	}

	public void AnimationFxCall (int c, int eventIndex)
	{
		if (fxDelegate != null)
		{
			AnimationInfo info = GetInfoByClip ((Clip) c);
			fxDelegate (info, info.getEvent (eventIndex));
		}

	}

	public void AnimationShakeCall (int c, int eventIndex)
	{
		AnimationInfo info = GetInfoByClip ((Clip) c);
		CameraController.instance.StartShake (info.getEvent (eventIndex).shakeTime, ()=>
		                                      {
			return CameraType.Follow;
		});
	}

	public void AnimationSpellAttackCall (int c, int eventIndex)
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



	#region
	public int[] SetAnimationIdex (string info)
	{
		string[] infos = info.Split (',');
		int c = int.Parse (infos[0]);
		int eventIndex = int.Parse (infos[1]);
		return new int[2] { c, eventIndex };
	}
	
	public void IdleMessage (string info)
	{
		IdleMessageCall ();
	}
	
	public void AttackMessage (string info)
	{
		int[] events = SetAnimationIdex (info);
		AttackMessageCall (events[0], events[1]);
	}
	
	public void AnimationMove (string info)
	{
		int[] events = SetAnimationIdex (info);
		AnimationMoveCall (events[0], events[1]);
	}
	
	public void AnimationFx (string info)
	{
		int[] events = SetAnimationIdex (info);
		AnimationFxCall (events[0], events[1]);
	}
	
	public void AnimationShake (string info)
	{
		int[] events = SetAnimationIdex (info);
		AnimationShakeCall (events[0], events[1]);
	}
	
	public void AnimationSpellAttack (string info)
	{
		int[] events = SetAnimationIdex (info);
		AnimationSpellAttackCall (events[0], events[1]);
	}
	
	public void DeathDoneMessage (string info)
	{
		AnimationDeath ();
	}
	#endregion

	#region Normal Attack
	private bool isNormalAttacking = false;
	private int queueId = -1;
	private int maxAttackCount { get { return normalAttack.Count; } }
	private float startTime = 0;
	private Clip curr_clip;
	private bool isAllowNext = false;
	public delegate void HandleNormalAttack (Clip clip);
	public void NormalAttack (HandleNormalAttack handleNormalAttack = null)
	{
		if (isNormalAttacking == false)
		{
			queueId++;
			//startTime = Time.time;
			isNormalAttacking = true;
			StartCoroutine (AttackQueue (handleNormalAttack));
		}
		if (curr_clip != Clip.Null && isAllowNext == true && Time.time - startTime > (GetInfoByClip (curr_clip).length + AntDefine.ANIMATION_OFFSET) / 2)
		{
			isAllowNext = false;
			//startTime = Time.time;
			queueId++;
		}
	}
	
	IEnumerator AttackQueue (HandleNormalAttack handleNormalAttack)
	{
		for (int i = 0; i <= Mathf.Min (queueId, maxAttackCount - 1); i++)
		{
			if (isFall || isHitted)
			{
				break;
			}
			curr_clip = normalAttack[i].clip;
			startTime = Time.time;
			isAllowNext = true;
			Play (curr_clip);
			if (handleNormalAttack != null)
			{
				handleNormalAttack (curr_clip);
			}
			yield return new WaitForSeconds (GetInfoByClip (curr_clip).length + AntDefine.ANIMATION_OFFSET);
		}
		isAllowNext = isAllowNext;
		curr_clip = Clip.Null;
		queueId = -1;
		isNormalAttacking = false;
	}
	#endregion
}
