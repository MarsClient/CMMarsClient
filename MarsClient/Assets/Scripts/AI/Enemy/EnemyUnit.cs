using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnit : HitUnit {

	public static List<EnemyUnit> enemysUnit = new List<EnemyUnit> ();
	AiEnemy m_enemy;

	public GameMonster gameMonster;

	protected void Awake ()
	{
		base.Awake ();
		//m_ac = GetComponent <AiAnimation>();
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

	public override void InitUI (GameBase gb)
	{
		this.gameMonster = (GameMonster) gb;
		base.InitUI (gb);
	}

	public override void ExtraEvent (AnimationInfo info, FrameEvent fe, int dmg)
	{
//		Debug.Log (info.clip);
//		Debug.Log (fe.antDisatnce);
//		Debug.Log (fe.antMoveSpd);
//		Debug.Log (fe.method);
		gameMonster.hp -= dmg;
		gameMonster.hp = Mathf.Max (gameMonster.hp, 0);
		slider.value = gameMonster.hpRatio;

		m_enemy.aiPath.Stop ();
		m_ac.aiMove.startMoveDir (info, fe);

		if (gameMonster.hp <= 0)
		{
			Remove ();
			m_ac.Play (Clip.Die);
			bloodBar.gameObject.SetActive (false);
		}
	}

	public override void UnitDeath ()
	{
		AlphaTween ();
		m_enemy.Stop ();
	}
}
