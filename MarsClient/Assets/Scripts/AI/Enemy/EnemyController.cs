using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(AIPath))]
public class EnemyController : MonoBehaviour {

	public static List<EnemyController> enemys = new List<EnemyController>();
	public Transform hitPos;
	public Transform target;
	public float attDistance = 2;

	private AIPath m_AIPath;
	private bool isAiWalk = true;


	private AnimationController animationController;



	void OnDisable ()
	{
		animationController.attackEvent -= AttackEvent;
	}

	public void Remove ()
	{
		enemys.Remove (this);
		CancelInvoke ("UpdateEnemyAI");
	}

	void Start () 
	{
		animationController = GetComponent <AnimationController>();
		animationController.attackEvent += AttackEvent;

		m_AIPath = GetComponent <AIPath>();
		enemys.Add (this);
		InvokeRepeating ("UpdateEnemyAI", 0, 3);
	}

	public void Hitted (AnimationItem animationItem, PlayerController player)
	{
		/*if (animationController.isFall)
		{
			return;
		}*/

		//stop move
		m_AIPath.Stop ();

		Vector3 forward = player.transform.position - transform.position;

		//hit animation
		if (animationController.isFall == false)
		{
			animationController.Play (animationItem.targetClip);
		}
		transform.forward = forward;
		ObjectPool.Instance.LoadObject ("EF/EF0001", hitPos.position);

		//hit move
		Vector3 targetPos = transform.position - forward.normalized * animationItem.attackMove;
		TweenPosition.Begin (gameObject, 0.25f, targetPos).onUpdate = onUpdate;

		//hit color
		CancelInvoke ("ResetColor");
		SetColor (0.8f);
		Invoke ("ResetColor", 0.1f);
	}

	void onUpdate ()
	{
		//Debug.Log ("qoiwueoqiuweoquiwe");
		m_AIPath.navAgent.SetDestination (transform.position);
	}

	void ResetColor ()
	{
		SetColor (0);
	}

	void SetColor (float a)
	{
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
				Color c = m.color;
				c.a = a;
				m.color = c;
			}
		}
	}
	
	void UpdateEnemyAI () 
	{
		if (animationController.doNotMove == false)
		{
			m_AIPath.StartPath (target.position, OnStartPath, OnPathCompleteToAttack);
			/*if (isAiWalk)
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
			}*/
		}
	}
    void OnStartPath ()
	{
		animationController.Play (Clip.Run);
	}

	void OnPathComplete ()
	{
		animationController.Play (Clip.Idle);
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
				animationController.Play (Clip.Attack1);
				return;
			}
		}
		animationController.Play (Clip.Idle);
	}

	void AttackEvent (AnimationItem animationItem)
	{
		for (int i = 0; i < PlayerUnit.playersUnit.Count; i++)
		{
			PlayerUnit pu = PlayerUnit.playersUnit[i];
			float angle = FightMath.GetMultiplyVector (transform, pu.transform);
			float distance = FightMath.DistXZ (transform.position, pu.transform.position);
			//Debug.Log (angle + "_____" + distance);
			if ((angle > 0 && distance < attDistance))
			{
				pu.Hitted (animationItem);
			}
		}
	}

	/*void SetMove (Vector3 targetPos)
	{
		Debug.Log (targetPos);
		bool isSame = (lastPos == target.position);
		if (animationController.doNotMove == false)
		{
			if (!isSame)
			{
				//New path
				isIdle = true;
				isRun = true;
				lastPos = target.position;
				nav.destination = lastPos;
			}
			if (nav.remainingDistance == 0)
			{
				return;
			}
			Debug.Log (nav.stoppingDistance + "___" + FightMath.DistXZ (transform.position, lastPos));
			if (FightMath.DistXZ (transform.position, lastPos) > nav.stoppingDistance)
			{
				if (isRun)
				{
					isRun = false;
					animationController.Play (Clip.Run);
				}
			}
			else
			{
				if (isIdle)
				{
					animationController.Play (Clip.Idle);
					isIdle = false;
				}
			}
		}
	}*/
}
