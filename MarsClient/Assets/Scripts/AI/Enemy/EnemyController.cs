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

	public void Hitted (Clip c, PlayerController player)
	{
		animationController.Play (c);
		transform.forward = (player.transform.position - transform.position);
		ObjectPool.Instance.LoadObject ("EF/EF0001", hitPos.position);

		CancelInvoke ("ResetColor");
		SetColor (Color.red);
		Invoke ("ResetColor", 0.1f);
	}

	void ResetColor ()
	{
		SetColor (Color.white);
	}

	void SetColor (Color c)
	{
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
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
		bool isSame = (lastPos == target.position);
		if (!isSame)
		{
			isIdle = true;
			isRun = true;
			lastPos = target.position;
			nav.destination = lastPos;
		}
		if (nav.remainingDistance == 0)
		{
			return;
		}
		if (nav.remainingDistance <= nav.stoppingDistance)
		{
			if (isIdle)
			{
				Debug.Log ("haah");
				animationController.Play (Clip.Idle);
				isIdle = false;
			}
		}
		else
		{
			if (isRun)
			{
				isRun = false;
				animationController.Play (Clip.Run);
			}
		}
		//if (nav.isPathStale)
	}
}
