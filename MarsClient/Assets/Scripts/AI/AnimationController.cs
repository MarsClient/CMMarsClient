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
	Hit,
	Fall,
	Die,
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
	public Clip targetClip = Clip.Null;
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

	public bool isHitted
	{
		get
		{
			return currentAnimationItem.clip == Clip.Hit;
		}
	}

	public bool isFall
	{
		get
		{
			return currentAnimationItem.clip == Clip.Fall;
		}
	}

	public bool doNotMove
	{
		get{
			return isAttack || isHitted || isFall;
		}
	}

	public float GetLength (Clip clip)
	{
		//Debug.Log (antPools[clip].animationClip.length);
		return antPools[clip].animationClip.length / animation[antPools[clip].clipName].speed;
	}

	void Start ()
	{
		if (trails != null && trails.Count > 0)
		{
			for (int j = 0; j < trails.Count; j++) 
			{
				trails[j].StartTrail (.2f, 0.5f);
			}
		}
		foreach (AnimationItem ai in animationItems)
		{
			ai.Init (animation);
			antPools.Add (ai.clip, ai);
			/*if (this.name == "EE0001")
			{
				Debug.LogError (ai.clip);
			}*/
		}
		currentAnimationItem = antPools[Clip.Idle];
	}

	private Clip currentClip;
	public void Play (Clip clip)
	{
		if (currentClip != clip)
		{
			if (antPools.ContainsKey (clip) == false)
			{
				return;
			}
			currentClip = clip;
			currentAnimationItem = antPools[clip];
			animation.CrossFade (currentAnimationItem.clipName);
		}
	}

	public void IdleMessage ()
	{
		Play (Clip.Idle);
	}

	void AttackMessage ()
	{
		//Debug.Log (currentAnimationItem.clipName);
		if (attackEvent != null)
		{
			attackEvent (currentAnimationItem);
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
		if (trails != null && trails.Count > 0)
		{
//			Debug.Log (trails.Count);
			RunAnimations ();
		}
	}	

	void RunAnimations ()
	{
		for (int j = 0; j < trails.Count; j++) 
		{
			trails[j].hideMeshRenderActive (isAttack);
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
				animation.Sample ();
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
}
