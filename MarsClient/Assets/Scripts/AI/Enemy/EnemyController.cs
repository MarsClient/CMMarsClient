using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(AIPath))]
public class EnemyController : MonoBehaviour {

	public static List<EnemyController> enemys = new List<EnemyController>();

	public Transform hitPos;


	private AnimationController animationController;

	public void Remove ()
	{
		enemys.Remove (this);
		CancelInvoke ("UpdateEnemyAI");
	}

	void Start () 
	{
		animationController = GetComponent <AnimationController>();
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
		Vector3 targetPos = transform.position - forward.normalized * animationItem.actionMove;
		TweenPosition.Begin (gameObject, 0.25f, targetPos);

		//hit color
		CancelInvoke ("ResetColor");
		SetColor (0.8f);
		Invoke ("ResetColor", 0.1f);
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


	private AIPath m_AIPath;
	public Transform target;

	private bool isAiWalk = true;
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
		Debug.Log ("Idle");
		animationController.Play (Clip.Idle);
	}

	void OnPathCompleteToAttack ()
	{
		Debug.Log ("Attack1");
		animationController.Play (Clip.Attack1);
		isAiWalk = true;
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
