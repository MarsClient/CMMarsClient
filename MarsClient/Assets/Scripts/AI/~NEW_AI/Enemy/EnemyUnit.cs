using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnit : HitUnit {

	public static List<EnemyUnit> enemysUnit = new List<EnemyUnit> ();
	AiEnemy m_enemy;
	
	void Awake ()
	{
		m_ac = GetComponent <AiAnimation>();
		m_enemy = GetComponent <AiEnemy> ();
		enemysUnit.Add (this);
	}
	
	void Remove ()
	{
		enemysUnit.Remove (this);
	}

	public override void ExtraEvent (AnimationInfo info, FrameEvent fe)
	{
//		Debug.Log (info.clip);
//		Debug.Log (fe.antDisatnce);
//		Debug.Log (fe.antMoveSpd);
//		Debug.Log (fe.method);

		m_enemy.aiPath.Stop ();
		m_ac.aiMove.startMoveDir (info, fe);
	}
}
