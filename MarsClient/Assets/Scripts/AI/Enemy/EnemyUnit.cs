using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnit : HitUnit {

	public static List<EnemyUnit> enemysUnit = new List<EnemyUnit> ();
	AiEnemy m_enemy;

	public GameMonster gameMonster;

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

	public void Update ()
	{
		updateUIShow ();
	}

	public override void Init (GameBase gb)
	{
		this.gameMonster = (GameMonster) gb;
		base.Init (gb);
	}

	public override void ExtraEvent (AnimationInfo info, FrameEvent fe, int dmg)
	{
//		Debug.Log (info.clip);
//		Debug.Log (fe.antDisatnce);
//		Debug.Log (fe.antMoveSpd);
//		Debug.Log (fe.method);
		gameMonster.hp -= dmg;
		gameMonster.hp = Mathf.Max (gameMonster.hp);
		slider.value = gameMonster.hpRatio;

		m_enemy.aiPath.Stop ();
		m_ac.aiMove.startMoveDir (info, fe);

		if (dmg <= 0)
		{

		}
	}
}
