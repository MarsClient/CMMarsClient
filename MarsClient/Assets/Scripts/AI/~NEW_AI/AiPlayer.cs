using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AiAnimation))]
public class AiPlayer : MonoBehaviour 
{
	public float attDistance = 2;

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
		if (aiAnt == null || aiAnt.aiMove == null) return;
		aiAnt.aiMove.moveEvent -= MoveEvent;
		aiAnt.attackDelegate -= AttackDelegate;
	}

	#region Move
	private Vector3 lastPos;
	void MoveEvent (AiMove aiMove)
	{
		AiMove.MoveState moveState = aiMove.currentMoveState;
		if (aiAnt.dontMove) { aiMove.currentMoveState = AiMove.MoveState.Stop; }
		if (moveState != AiMove.MoveState.SpecialMoving && aiAnt.dontMove == false)
		{
			Clip  c = (moveState == AiMove.MoveState.Moving) ? Clip.Run : Clip.Idle;

			//input move
			aiAnt.Play (c);
			if (lastPos != transform.position)
			{
				lastPos = transform.position;
				PlayerStateNet (c);
				//Debug.Log ("______" + c);
			}
		}
		else
		{
			//About attack
		}
	}
	#endregion

	#region Player State .Net
	void PlayerStateNet (Clip c)
	{
		if (Main.Instance.role != null)
		{
			//Player p = new Player();

			Main.Instance.role.x = (float) transform.position.x;
			Main.Instance.role.z = (float) transform.position.z;
			Main.Instance.role.xRo = (float) transform.forward.x;
			Main.Instance.role.zRo = (float) transform.forward.z;
			Main.Instance.role.action = (int)c;
			//Debug.LogError (transform.forward);
			NetSend.SendUpdatePlayer (Main.Instance.role);
		}
	}
	#endregion

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
			PlayerStateNet (clip);
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
		PlayerStateNet (Clip.Spell1);
	}

	public void ShootSpell2 ()
	{
		aiAnt.Play (Clip.Spell2);
		PlayerStateNet (Clip.Spell2);
	}
	#endregion

	#region AiAnimation Event
	void AttackDelegate (AnimationInfo info, FrameEvent fe)
	{
		for (int i = 0; i < EnemyUnit.enemysUnit.Count; i++)
		{
			EnemyUnit eu = EnemyUnit.enemysUnit[i];
			float angle = FightMath.GetMultiplyVector (transform, eu.transform);
			float distance = FightMath.DistXZ (transform.position, eu.transform.position);
			//Debug.Log (angle + "_____" + distance);
			if ((angle > 0 && distance < attDistance) || (angle <= 0 && distance < attDistance / 4))
			{
				FightMath.SetTargetForwardDirection (eu.transform, transform);
				eu.Hitted (info, fe);
			}
		}
	}
	#endregion

	#region Hit Enemy
	void attackEvent (AnimationInfo info)
	{

	}
	#endregion
}
