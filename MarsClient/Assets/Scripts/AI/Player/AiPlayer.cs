using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AiAnimation))]
public class AiPlayer : MonoBehaviour 
{

	/*Effect Position*/
	public Transform groudTranform;
	public Transform assaultTranform;


	public AttType attType = AttType.inf;
	public PRO pro = PRO.ZS;

	public float attDistance = 2;

	//private AiMove aiMove;
	private AiAnimation aiAnt;
	void Start ()
	{
		//aiMove = GetComponent<AiMove> ();
		aiAnt  = GetComponent<AiAnimation>();
		aiAnt.aiMove.moveEvent += MoveEvent;
		aiAnt.attackDelegate += AttackDelegate;
		aiAnt.spellAttackDelegate += SpellAttackDelegate;
		aiAnt.fxDelegate += FxDelegate;
	}
	void OnDisable ()
	{
		if (aiAnt == null || aiAnt.aiMove == null) return;
		aiAnt.aiMove.moveEvent -= MoveEvent;
		aiAnt.attackDelegate -= AttackDelegate;
		aiAnt.spellAttackDelegate -= SpellAttackDelegate;
		aiAnt.fxDelegate -= FxDelegate;
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
		if (Main.Instance!=null && Main.Instance.role != null)
		{
			//Player p = new Player();
			Role role = new Role();
			role.roleId = Main.Instance.role.roleId;
			role.accountId = Main.Instance.role.accountId;
			role.x = (float) transform.position.x;
			role.z = (float) transform.position.z;
			role.xRo = (float) transform.forward.x;
			role.zRo = (float) transform.forward.z;
			role.action = (int)c;
			if (PublicZoneManager.instance)
			{
				NetSend.SendUpdatePlayer (role);
			}
			else if (FightManager.instance)
			{
				NetSend.SendTeamUpdate (role);
			}
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

	public void ShootSpell3 ()
	{
		aiAnt.Play (Clip.Spell3);
		PlayerStateNet (Clip.Spell3);
	}
	#endregion

	#region AiAnimation Event
	void AttackDelegate (AnimationInfo info, FrameEvent fe)
	{
		if (attType == AttType.inf)
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
					eu.Hitted (info, fe, Main.Instance.role.attNormalDmg, Main.Instance.role.isDouble, true);
				}
			}
		}
		else if (attType == AttType.bow)
		{
			//Shoot
			//GameObject res_Go = PoolManager.Instance.LoadGameObject ("Bullets_10001");//Resources.Load ("Bullets_10001") as GameObject;
			GameObject go = PoolManager.Instance.LoadGameObject (GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle, transform);
			//GameObject.Instantiate (res_Go, transform.position, transform.rotation) as GameObject;
			BulletsSample bs = go.GetComponent <BulletsSample>();
			if (bs != null)
			{
				bs.InitBullets ((HitUnit hu)=> 
				{
//					Debug.Log (Main.Instance.role.attNormalDmg + "_" + Main.Instance.role.isDouble);
					hu.Hitted (info, fe, Main.Instance.role.attNormalDmg, Main.Instance.role.isDouble, true);
				});
				bs.InitLayer (TagLayerDefine.ENEMY_TAG);
			}

		}
	}


	void SpellAttackDelegate (AnimationInfo info, FrameEvent fe)
	{
		if (info.clip == Clip.Spell1)
		{
			StartCoroutine (ShootMulBullet (7, 0.1f, 2, info, fe));
		}
		else if (info.clip == Clip.Spell2)
		{
			GameObject go = PoolManager.Instance.LoadGameObject (GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle, transform);
			//GameObject.Instantiate (res_Go, transform.position, transform.rotation) as GameObject;
			BulletsSample bs = go.GetComponent <BulletsSample>();
			if (bs != null)
			{
				bs.InitBullets ((HitUnit hu)=> 
				                {
					hu.Hitted (info, fe, 10, false, true);
				});
				bs.InitLayer (TagLayerDefine.ENEMY_TAG);
			}
		}
	}

	IEnumerator ShootMulBullet (int count, float interval, float randge, AnimationInfo info, FrameEvent fe)
	{
		string asserbundle = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle;
		for (int i = 0; i < count; i++)
		{
			yield return new WaitForSeconds (interval);
			GameObject go = PoolManager.Instance.LoadGameObject (GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle);
			BulletsSample bs = go.GetComponent <BulletsSample>();
			if (bs != null)
			{
				bs.InitBullets ((HitUnit hu)=> 
				                {
					hu.Hitted (info, fe, 10, false, true);
				}, true);
				bs.InitPosition (FightMath.TargetRandge (transform, randge));
				bs.InitLayer (TagLayerDefine.ENEMY_TAG);
			}
		}
	}

	#endregion

	#region Fx
	void FxDelegate (AnimationInfo info, FrameEvent fe)
	{
		GameEffect ge = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip);
		if (ge != null)
		{
			GameObject go = PoolManager.Instance.LoadGameObject (ge.assetbundle);
			if (go == null)
			{
				return;
			}
			if (ge.fxType == FxType.Groud)
			{
				go.transform.position = groudTranform.position;
			}
			else if (ge.fxType == FxType.Parent)
			{
				go.transform.parent = assaultTranform;
				go.transform.localPosition = Vector3.zero;
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
