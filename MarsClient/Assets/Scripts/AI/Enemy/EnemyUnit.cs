using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyUnit : HitUnit {

	public static List<EnemyUnit> enemysUnit = new List<EnemyUnit> ();
	AiEnemy m_enemy;

	public GameMonster gameMonster {get; private set; }

	protected void Awake ()
	{
		PhotonClient.processResults += ProcessResults;
		PhotonClient.processResultSync += ProcessResultSync;


		base.Awake ();
		//m_ac = GetComponent <AiAnimation>();
		m_enemy = GetComponent <AiEnemy> ();
		enemysUnit.Add (this);
	}

	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
		PhotonClient.processResultSync -= ProcessResultSync;
	}
	
	void Remove ()
	{
		enemysUnit.Remove (this);
	}

	public void Update ()
	{
		updateUIShow ();
	}

	public void InitUI (GameMonster gb)
	{
		this.gameMonster = gb;
		base.InitUIVal (gb.level);
	}

	public override void ExtraEvent (int dmg)
	{
		/*gameMonster.hp -= dmg;
		gameMonster.hp = Mathf.Max (gameMonster.hp, 0);
		*/

		GameMonster gm = new GameMonster();
		gm.id = gameMonster.id;
		gm.deductHp = dmg;
		NetSend.SendMonsterUpdate (gm);
	}

	public override void UnitDeath ()
	{
		AlphaTween ();
		m_enemy.Stop ();
	}

	public void DeathDoSomething()
	{
	}

	void HpDeduct ()
	{
		//hp deduct
		slider.value = gameMonster.hpRatio;
		m_enemy.aiPath.Stop ();

		if (gameMonster.hp <= 0)
		{
			Remove ();
			m_ac.Play (Clip.Die);
			TweenPosition.Begin (GetComponentInChildren<Animation>().gameObject, 0.5f, Vector3.zero);
			bloodBar.gameObject.SetActive (false);
			
			DeathDoSomething ();
		}
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.MonsterStateUpdate)
		{
			if (gameMonster.id == bundle.gameMonster.id)
			{
				gameMonster.hp = bundle.gameMonster.hp;

				HpDeduct ();
				this.HitEffect ();
			}
		}
	}

	void ProcessResultSync (Bundle bundle)
	{

	}
}
