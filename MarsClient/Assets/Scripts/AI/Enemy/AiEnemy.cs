using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AiAnimation))]
[RequireComponent(typeof(AIPath))]
public class AiEnemy : MonoBehaviour 
{
	public static List<AiEnemy> enemys = new List<AiEnemy>();
	public Transform target;
	public float attDistance = 2;

	private AIPath m_AIPath;
	public AIPath aiPath { get { return m_AIPath; } }
	private bool isAiWalk = true;
	private AiAnimation aiAnt;
	public AiAnimation AIAnt { get { return aiAnt; } }

	void OnDisable ()
	{
		aiAnt.attackDelegate -= AttackDelegate;
	}
	
	public void Remove ()
	{
		enemys.Remove (this);
		CancelInvoke ("UpdateEnemyAI");
	}

	void AttackDelegate (AnimationInfo info, FrameEvent fe)
	{
		for (int i = 0; i < PlayerUnit.playersUnit.Count; i++)
		{
			PlayerUnit pu = PlayerUnit.playersUnit[i];
			float angle = FightMath.GetMultiplyVector (transform, pu.transform);
			float distance = FightMath.DistXZ (transform.position, pu.transform.position);
			//Debug.Log (angle + "_____" + distance);
			if ((angle > 0 && distance < attDistance))
			{
				pu.Hitted (info, fe);
			}
//			Debug.LogError (i + "_____");
		}
	}

	void Start () 
	{
		aiAnt = GetComponent <AiAnimation>();
		aiAnt.attackDelegate += AttackDelegate;
		
		m_AIPath = GetComponent <AIPath>();
		enemys.Add (this);
		InvokeRepeating ("UpdateEnemyAI", 0, 3);
	}

	void UpdateEnemyAI () 
	{
		if (aiAnt.dontMove == false)
		{
			if (target != null)
			{
				m_AIPath.StartPath (target.position, OnStartPath, OnPathCompleteToAttack);
			}
			isAiWalk = FightMath.isStateRandom ();
			if (isAiWalk)
			{
				isAiWalk = false;
				m_AIPath.StartPath (FightMath.GetRandomVectorRun (m_AIPath.navAgent), OnStartPath, OnPathComplete);
			}
			else
			{
				if (target == null)
				{
					isAiWalk = true;
					return;
				}
				m_AIPath.StartPath (target.position, OnStartPath, OnPathCompleteToAttack);
			}
		}
	}

	void OnStartPath ()
	{
		aiAnt.Play (Clip.Run);
	}
	
	void OnPathComplete ()
	{
		aiAnt.Play (Clip.Idle);
	}

	void OnPathCompleteToAttack ()
	{
		isAiWalk = true;
		//find Nearest PlayerUnit
		ArrayList list = FightMath.findNearest (transform);
		if (list[0] != null)
		{
			PlayerUnit playerUnit = (PlayerUnit) list[0];
			float distance = (float) list[1];
			if (distance <= attDistance)
			{
				transform.forward = playerUnit.transform.position - transform.position;
				aiAnt.Play (Clip.Attack1);
				return;
			}
		}
		aiAnt.Play (Clip.Idle);
	}
}
