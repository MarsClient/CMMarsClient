using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPath : MonoBehaviour 
{
	public delegate void OnPathComplete ();
	public delegate void OnStartPath ();

	private NavMeshAgent _navAgent;
	public NavMeshAgent navAgent { get{ return _navAgent; } }

	void Start ()
	{
		_navAgent = GetComponent <NavMeshAgent>();
	}

	public void Stop ()
	{
		_navAgent.Stop (true);
		StopAllCoroutines ();
	}

	public void Resume ()
	{
		_navAgent.Resume ();
	}

	public void StartPath (Vector3 targetPos)
	{
		StartPath (targetPos, null, null);
	}

	public void StartPath (Vector3 targetPos, OnStartPath onStartPath, OnPathComplete onPathComplete)
	{
		StopAllCoroutines ();
		StartCoroutine (StartUpdatePath (targetPos, onStartPath, onPathComplete));
	}

	IEnumerator StartUpdatePath (Vector3 targetPos, OnStartPath onStartPath, OnPathComplete onPathComplete)
	{
		//Test
		//int protect = 0;

		_navAgent.destination = targetPos;
		bool isPathing = true;
		while (isPathing)
		{
//			if (protect++ > 1000)
//			{
//				isPathing = false;
//			}
			yield return new WaitForSeconds (0);

			//Debug.Log ("Update");
			if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
			{
				isPathing = false;
			}
			else
			{
				OnStartPathCallback (onStartPath);
			}
		}
		OnCompleteCallback (onPathComplete);
	}

	void OnCompleteCallback (OnPathComplete onPathComplete)
	{
		//Debug.Log ("Done");
		if (onPathComplete != null)
		{
			onPathComplete ();
		}
	}

	void OnStartPathCallback (OnStartPath onStartPath)
	{
		//Debug.Log ("Start");
		if (onStartPath != null)
		{
			onStartPath ();
		}
	}


	/*void Update () 
	{
		NavMeshPath path = new NavMeshPath();
		bool isWalkable = _navAgent.CalculatePath (transform.position, path);
		Debug.Log (isWalkable);
		int button = 0;
		if(Input.GetMouseButtonDown(button)) {
			Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) 
			{
				Vector3 targetPosition = hitInfo.point;
				StartPath (targetPosition, null, null);
			}
		}
	}

	void OnGUI ()
	{
		if (GUILayout.Button ("Srop"))
		{
			Stop ();
		}

		if (GUILayout.Button ("Resume"))
		{
			Resume ();
		}

		if (GUILayout.Button ("GoOn"))
		{
			StartPath (Vector3.zero, null, null);
		}
	}*/
}
