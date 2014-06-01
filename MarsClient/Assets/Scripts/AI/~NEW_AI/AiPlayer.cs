using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AiAnimation))]
public class AiPlayer : MonoBehaviour 
{
	//private AiMove aiMove;
	private AiAnimation aiAnt;
	void Start ()
	{
		//aiMove = GetComponent<AiMove> ();
		aiAnt  = GetComponent<AiAnimation>();
		aiAnt.aiMove.moveEvent += MoveEvent;
		aiAnt.attackDelegate += AttackDelegate;
	}
	void OnDisable ()
	{
		if (aiAnt.aiMove == null || aiAnt == null) return;
		aiAnt.aiMove.moveEvent -= MoveEvent;
		aiAnt.attackDelegate -= AttackDelegate;
	}

	#region Move
	void MoveEvent (AiMove aiMove)
	{
		AiMove.MoveState moveState = aiMove.currentMoveState;
		if (aiAnt.dontMove) { aiMove.currentMoveState = AiMove.MoveState.Stop; }
		if (moveState != AiMove.MoveState.SpecialMoving && aiAnt.dontMove == false)
		{
			//input move
			aiAnt.Play ((moveState == AiMove.MoveState.Moving) ? Clip.Run : Clip.Idle);
		}
		else
		{
			//About attack
		}
	}
	#endregion

/*#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
	void Update ()
	{
		if (Input.GetMouseButton (0))
		{
			NormalAttack ();
		}
	}
#elif UNITY_ANDROID || UNITY_IPHONE
	public void MobliePlatform ()
	{
		NormalAttack ();
	}
#endif*/

	#region About Attack
	private bool isNormalAttacking = false;
	private int queueId = -1;
	private int maxAttackCount { get { return aiAnt.normalAttack.Count; } }
	private float startTime = 0;
	private Clip clip;
	public void NormalAttack ()
	{
		if (isNormalAttacking == false)
		{
			queueId++;
			startTime = Time.time;
			isNormalAttacking = true;
			StartCoroutine (AttackQueue ());
		}
		if (clip != Clip.Null && Time.time - startTime > aiAnt.GetInfoByClip (clip).length / 2)
		{
			startTime = Time.time;
			queueId++;
		}
	}

	IEnumerator AttackQueue ()
	{
		for (int i = 0; i <= Mathf.Min (queueId, maxAttackCount - 1); i++)
		{
			if (aiAnt.isFall || aiAnt.isHitted)
			{
				break;
			}
			clip = aiAnt.normalAttack[i].clip;
			aiAnt.Play (clip);
			yield return new WaitForSeconds (aiAnt.GetInfoByClip (clip).length);
		}
		clip = Clip.Null;
		queueId = -1;
		isNormalAttacking = false;
	}
	#endregion

	#region spell
	public void ShootSpell1 ()
	{
		aiAnt.Play (Clip.Spell1);
	}

	public void ShootSpell2 ()
	{
		aiAnt.Play (Clip.Spell2);
	}
	#endregion

	#region AiAnimation Event
	void AttackDelegate (AnimationInfo info)
	{
		//aiMove.startMoveDir (info);
	}
	#endregion
}
