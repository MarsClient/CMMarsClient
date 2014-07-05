using UnityEngine;
using System.Collections;
using URandom = UnityEngine.Random;

public enum NpcMonsterState
{
	Null,
	Idle,
	Run,
	Attack,//npc ignore
}

public class FightMath
{
	public static void setRota (Transform mTransform)
	{
		mTransform.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
	}

	public static float GetMultiplyVector  (Transform att, Transform def)
	{
		Vector3 a = att.transform.position;//py
		Vector3 d = def.transform.position;//ec
		Vector3 forward = d - a;
		return Vector3.Dot (att.forward, forward);
	}

	public static float DistXZ (Vector3 l, Vector3 r)
	{
		l.y = 0;
		r.y = 0;
		float dist = Mathf.Sqrt (Vector3.SqrMagnitude (l - r));
		return dist;
	}

	public static Vector3 GetRandomVectorRun (NavMeshAgent nav)
	{
		Transform m_tra = nav.transform;
		float x = URandom.Range (-nav.stoppingDistance * 2, nav.stoppingDistance * 2);
		float y = nav.transform.position.y;
		float z = URandom.Range (-nav.stoppingDistance * 2, nav.stoppingDistance * 2);
		return new Vector3 (x, y, z);
	}

	public static ArrayList findNearest (Transform ef)
	{
		ArrayList list = new ArrayList ();
		float min = float.MaxValue;
		PlayerUnit playerUnit = null;
		for (int i = 0; i < PlayerUnit.playersUnit.Count; i++)
		{
			PlayerUnit pu = PlayerUnit.playersUnit[i];
			float distance = FightMath.DistXZ (ef.position, pu.transform.position);
			if (min > distance)
			{
				min = distance;
				playerUnit = pu;
			}
		}
		list.Add (playerUnit);
		list.Add (min);
//		Debug.Log (list[0] + "<-----0______1----->" + list[1]);
		return list;//0-get the nearest player, 1-get the nearest distance
	}

	public static void SetTargetForwardDirection (Transform left, Transform right)
	{
		Vector3 forward = right.position - left.position;
		if (forward != Vector3.zero)
		{
			left.forward = forward.normalized;
		}
	}

	public static bool isStateRandom () { return Random.Range (0, 2) == 1; }
}
