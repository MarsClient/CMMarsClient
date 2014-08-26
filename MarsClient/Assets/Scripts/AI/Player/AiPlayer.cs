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

	private bool isOwn = false;

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

	public void SetOwn (bool own)//check is myself
	{
		isOwn = own;
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
			//role.accountId = Main.Instance.role.accountId;
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

	/**/
	public void NormalAttack ()
	{
		aiAnt.NormalAttack ((Clip clip)=>
		{
			PlayerStateNet (clip);
		});
	}

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
		//if (!isOwn) return;

		if (attType == AttType.inf)
		{
			for (int i = 0; i < EnemyUnit.enemysUnit.Count; i++)
			{
				EnemyUnit eu = EnemyUnit.enemysUnit[i];
				float angle = FightMath.GetMultiplyVector (transform, eu.transform);
				float distance = FightMath.DistXZ (transform.position, eu.transform.position);
				if ((angle > 0 && distance < attDistance) || (angle <= 0 && distance < attDistance / 4))
				{
					FightMath.SetTargetForwardDirection (eu.transform, transform);
					AnimationInfoCache cache = new AnimationInfoCache();
					cache.info = info;
					cache.fe = fe;
					cache.dmg = isOwn ? (Main.Instance.role.attNormalDmg) : 0;
					cache.isDouble = Main.Instance.role.isDouble;
					cache.isDmg = isOwn;
					eu.Hitted (cache/*info, fe, Main.Instance.role.attNormalDmg, Main.Instance.role.isDouble, true*/);
				}
			}
		}
		else if (attType == AttType.bow)
		{
			//Shoot
			string path = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle;
			PoolManager.Instance.LoadGameObject (path, (GameObject go)=>
			{
				go.transform.position = transform.position;
				go.transform.rotation = transform.rotation;
				BulletsSample bs = go.GetComponent <BulletsSample>();
				if (bs != null)
				{
					bs.InitBullets ((HitUnit hu)=> 
					                {
						AnimationInfoCache cache = new AnimationInfoCache();
						cache.info = info;
						cache.fe = fe;
						cache.dmg = Main.Instance.role.attNormalDmg;
						cache.isDouble = Main.Instance.role.isDouble;
						cache.isDmg = true;
						hu.Hitted (cache/*info, fe, Main.Instance.role.attNormalDmg, Main.Instance.role.isDouble, true*/);
					});
					bs.InitLayer (TagLayerDefine.ENEMY_TAG);
				}
			}, Constants.EF);


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
			string path = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle;
			PoolManager.Instance.LoadGameObject (path, (GameObject go)=>
			{
				go.transform.position = transform.position;
				go.transform.rotation = transform.rotation;
				BulletsSample bs = go.GetComponent <BulletsSample>();
				if (bs != null)
				{
					bs.InitBullets ((HitUnit hu)=> 
					                {
						//hu.Hitted (info, fe, 10, false, true);
					});
					bs.InitLayer (TagLayerDefine.ENEMY_TAG);
				}
			}, Constants.EF);

		}
	}

	IEnumerator ShootMulBullet (int count, float interval, float randge, AnimationInfo info, FrameEvent fe)
	{
		string asserbundle = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle;
		for (int i = 0; i < count; i++)
		{
			yield return new WaitForSeconds (interval);
			string key = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip).assetbundle;
			PoolManager.Instance.LoadGameObject (key, (GameObject go)=>
			{
				BulletsSample bs = go.GetComponent <BulletsSample>();
				if (bs != null)
				{
					bs.InitBullets ((HitUnit hu)=> 
					                {
						//hu.Hitted (info, fe, 10, false, true);
					}, true);
					bs.InitPosition (FightMath.TargetRandge (transform, randge));
					bs.InitLayer (TagLayerDefine.ENEMY_TAG);
				}
			}, Constants.EF);
		}
	}

	#endregion

	#region Fx
	void FxDelegate (AnimationInfo info, FrameEvent fe)
	{
		GameEffect ge = GameData.Instance.getGameEffectByAction ((int) pro, (int) info.clip);
		if (ge != null)
		{
			Transform target = null;
			Transform parent = null;
			if (ge.fxType == FxType.Groud)
			{
				target = groudTranform;
			}
			else if (ge.fxType == FxType.Parent)
			{
				target = assaultTranform;
				parent = assaultTranform;
			}
			PoolManager.Instance.LoadGameObject (ge.assetbundle, (GameObject g_obj)=>
			{
				if (target != null)
				{
					g_obj.transform.position = target.position;
				}
				if (parent != null)
				{
					g_obj.transform.parent = parent;
				}
			}, Constants.EF);
		}

	}
	#endregion

	#region Hit Enemy
	void attackEvent (AnimationInfo info)
	{

	}
	#endregion
}
