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
}
