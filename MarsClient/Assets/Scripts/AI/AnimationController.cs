using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AniClip : int
{
	Idle,
	Run,
	Attack1,
	Attack2,
	Attack3,
	Hit,
	Fall,
	Die,
	Null,
	Spell1,
	Spell2,
}

public class AnimationMessage
{
	public static string IDLE_MESSAGE = "IdleMessage";
	public static string ATTACK_MESSAGE = "AttackMessage";

}

[System.Serializable]
public class AnimationMessageNeed
{
	public float frame;
	public string eventMesage;
}

[System.Serializable]
public class AnimationItem
{
	public AniClip clip;
	public AniClip targetClip = AniClip.Null;
	public AnimationClip animationClip;
	public float speed = 1;
	public AnimationMessageNeed[] eventFrames;
	public float actionMove = 0;
	public float attackMove = 0;
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
		foreach (AnimationMessageNeed am in eventFrames)
		{
			SetEvent (am.eventMesage, am.frame / animationClip.frameRate);
		}
	}

	private void AddCompleteEvent ()
	{
		if (animationClip.wrapMode == WrapMode.Default || animationClip.wrapMode == WrapMode.Once)
		{
			////Debug.Log (animationClip.name);
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
		//animationEvent.objectReferenceParameter = 
		animationEvent.functionName = onEvent;
		animationEvent.intParameter = (int) clip;//clip.ToString();
		animationEvent.messageOptions = SendMessageOptions.RequireReceiver;
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

	public delegate void AttackAOEEvent (AnimationItem animationItem);
	public AttackAOEEvent attackAOEEvent;

	public delegate void AssaultEvent (AnimationItem animationItem);
	public AssaultEvent assaultEvent;

	private Animation m_Animation;
	public AnimationItem[] animationItems;
	private Dictionary<AniClip, AnimationItem> antPools = new Dictionary<AniClip, AnimationItem>();

	public AnimationItem currentAnimationItem;

	public bool isAttack
	{
		get
		{
			return currentAnimationItem.clip.ToString().Contains ("Attack");
		}
	}

	public bool isSpell
	{
		get
		{
			return currentAnimationItem.clip.ToString().Contains ("Spell");
		}
	}

	public bool isHitted
	{
		get
		{
			return currentAnimationItem.clip == AniClip.Hit;
		}
	}

	public bool isFall
	{
		get
		{
			return currentAnimationItem.clip == AniClip.Fall;
		}
	}

	public bool doNotMove
	{
		get{
			return isAttack || isHitted || isFall || (isSpell && currentAnimationItem.clip != AniClip.Spell2);
		}
	}

	public float GetLength (AniClip clip)
	{
		////Debug.Log (antPools[clip].animationClip.length);
		return antPools[clip].animationClip.length / m_Animation[antPools[clip].clipName].speed;
	}

	void Start ()
	{
		m_Animation = GetComponentInChildren<Animation>();
		if (trails != null && trails.Count > 0)
		{
			for (int j = 0; j < trails.Count; j++) 
			{
				trails[j].StartTrail (.2f, 0.5f);
			}
		}
		foreach (AnimationItem ai in animationItems)
		{
			ai.Init (m_Animation);
			antPools.Add (ai.clip, ai);
			/*if (this.name == "EE0001")
			{
				//Debug.LogError (ai.clip);
			}*/
		}
		currentAnimationItem = antPools[AniClip.Idle];
	}

	private AniClip currentClip;
	public void Play (AniClip clip)
	{
		if (currentClip != clip)
		{
			if (antPools.ContainsKey (clip) == false)
			{
				return;
			}
			currentClip = clip;
			currentAnimationItem = antPools[clip];
			m_Animation.CrossFade (currentAnimationItem.clipName);
		}
	}

	public void IdleMessage (int c)
	{
		//Debug.Log ("Idle");
		Play (AniClip.Idle);
	}

	public void AttackMessage (int c)
	{
		if (attackEvent != null)
		{
			//Debug.Log (c);
			attackEvent (antPools[(AniClip)c]);
		}
	}

	public void AttackAOEMessage (int c)
	{
		if (attackAOEEvent != null)
		{
			//Debug.Log (c);
			attackAOEEvent (antPools[(AniClip)c]);
		}
	}

	public void SpellAssault (int c)
	{
		if (assaultEvent != null)
		{
			assaultEvent (antPools[(AniClip)c]);
		}
	}

	/*
	 *New WeaponTrail
	 *
	 */
	protected float t = 0.033f;
	private float tempT = 0;
	protected float m = 0;
	protected Vector3 lastEulerAngles = Vector3.zero;
	protected Vector3 lastPosition = Vector3.zero;
	protected Vector3 eulerAngles = Vector3.zero;
	protected Vector3 position = Vector3.zero;
	protected float animationIncrement = 0.003f; // ** This sets the number of time the controller samples the animation for the weapon trails

	public List<WeaponTrail> trails = new List<WeaponTrail>();

	void LateUpdate ()
	{
		//Set Frame Done
//		int currentFrame = (int) (m_Animation[currentAnimationItem.clipName].time * currentAnimationItem.animationClip.frameRate);
//		//Debug.Log (currentFrame);

		if (trails != null && trails.Count > 0)
		{
			RunAnimations ();
		}
	}	

	void RunAnimations ()
	{
		for (int j = 0; j < trails.Count; j++) 
		{
			trails[j].hideMeshRenderActive (isAttack || isSpell);
		}

		if (t > 0)
		{
			eulerAngles = transform.eulerAngles;
			position = transform.localPosition;

			while (tempT < t) 
			{
				tempT += animationIncrement;
				m = tempT / t;
				transform.eulerAngles = new Vector3(Mathf.LerpAngle(lastEulerAngles.x, eulerAngles.x, m),Mathf.LerpAngle(lastEulerAngles.y, eulerAngles.y, m),Mathf.LerpAngle(lastEulerAngles.z, eulerAngles.z, m));
				transform.position = Vector3.Lerp(lastPosition, position, m);
				m_Animation.Sample ();
				for (int j = 0; j < trails.Count; j++) {
					if (trails[j].time > 0) {
						trails[j].Itterate (Time.time - t + tempT);
					} else {
						trails[j].ClearTrail ();
					}
				}
			}
			//
			// ** End of loop
			//
			tempT -= t;
			//
			// ** Sets the position and rotation to what they were originally
			transform.localPosition = position;
			transform.eulerAngles = eulerAngles;
			lastPosition = position;
			lastEulerAngles = eulerAngles;
			//
			// ** Finally creates the meshes for the WeaponTrails (one per frame)
			//
			for (int j = 0; j < trails.Count; j++) {
				if (trails[j].time > 0) {
					trails[j].UpdateTrail (Time.time, t);
				}
			}
		}
	}

	public bool isPlayer = false;
	void OnGUI ()
	{
		//DebugConsole.Log (transform.position);
		if (isPlayer)
		{
			if (GUILayout.Button ("Spell1"))
			{
				Play (AniClip.Spell1);
			}
			if (GUILayout.Button ("Spell2"))
			{
				Play (AniClip.Spell2);
			}
		}
	}
}
