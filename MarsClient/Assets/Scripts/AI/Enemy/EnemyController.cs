using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {

	public static List<EnemyController> enemys = new List<EnemyController>();

	public Transform hitPos;


	private AnimationController animationController;

	public void Remove ()
	{
		enemys.Remove (this);
	}

	void Start () 
	{
		animationController = GetComponent <AnimationController>();
		nav = GetComponent <NavMeshAgent>();
		enemys.Add (this);
	}

	public void Hitted (AnimationItem animationItem, PlayerController player)
	{
		if (animationController.isFall)
		{
			return;
		}

		Vector3 forward = player.transform.position - transform.position;
		//hit move
		Vector3 targetPos = transform.position - forward.normalized * animationItem.actionMove;
		TweenPosition.Begin (gameObject, 0.25f, targetPos);

		//hit animation
		animationController.Play (animationItem.targetClip);
		transform.forward = forward;
		ObjectPool.Instance.LoadObject ("EF/EF0001", hitPos.position);

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


	private NavMeshAgent nav;
	public Transform target;
	private Vector3 lastPos = Vector3.one * 1000000;
	private bool isIdle = false;
	private bool isRun = false;

	void Update () 
	{
		SetMove ();
		//if (nav.isPathStale)
	}

	void SetMove ()
	{
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
			//Debug.Log (nav.remainingDistance + "___" + DistXZ (transform.position, lastPos));
			if (DistXZ (transform.position, lastPos) > nav.stoppingDistance)
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
	}

	float DistXZ (Vector3 l, Vector3 r)
	{
		l.y = 0;
		r.y = 0;
		float dist = Mathf.Sqrt (Vector3.SqrMagnitude (l - r));
		return dist;
	}
}
