using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(FPSInputController))]
public class PlayerController : MonoBehaviour {

	public PRO pro = PRO.ZS;

	public float attDistance = 1;

	[HideInInspector]
	public AnimationController animationController;
	[HideInInspector]
	public FPSInputController fpsController;

	void OnEnable ()
	{
		FPSInputController.inputController += inputController;
		FPSInputController.attackController += attackController;
		FPSInputController.assaultDelegate += assaultDelegate;
	}

	void OnDisable ()
	{
		FPSInputController.inputController -= inputController;
		FPSInputController.attackController -= attackController;
		FPSInputController.assaultDelegate -= assaultDelegate;
		if (animationController == null)
		{
			return;
		}
		animationController.attackEvent -= attackEvent;
		animationController.attackAOEEvent -= AttackAOEMessage;
		animationController.assaultEvent -= assaultEvent;
	}

	private Vector3 lastPosMove;
	private Vector3 lastPos;
	void inputController (FPSInputController fpsController)
	{
		fpsController.canControl = !animationController.doNotMove;
		if (fpsController.canControl == false)
		{
			return;
		}
		//Move
		Clip c = Clip.Null;

		bool isStop = (lastPosMove != fpsController.directionVector);
		bool isMoving = (transform.position != lastPos);
		lastPos = transform.position;
		lastPosMove = fpsController.directionVector;

		if (fpsController.directionVector.x != 0 || fpsController.directionVector.z != 0)
		{
			if (animationController.isSpell == false)
			{
				animationController.Play (Clip.Run);
				c = Clip.Run;
			}
		}
		else
		{
			if (animationController.isSpell == false)
			{
				animationController.Play (Clip.Idle);
				c =Clip.Idle;
			}
		}

		if (isMoving || isStop)
		{
//			Debug.Log (transform.forward);
			Player p = new Player();
			p.uniqueId = Main.Instance.account.uniqueId;
			p.x = transform.position.x;
			p.z = transform.position.z;
			p.xRo = transform.forward.x;
			p.zRo = transform.forward.z;
			p.actionId = (int)c;
			p.roleName = Main.Instance.account.roleName;
			NetSend.SendUpdatePlayerPos (p); 
		}
	}

	void attackController (FPSInputController fpsController)
	{
		Attack ();
	}

	void attackEvent (AnimationItem animationItem)
	{
		fpsController.moveDir (animationItem);
		for (int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			float angle = FightMath.GetMultiplyVector (transform, ec.transform);
			float distance = FightMath.DistXZ (transform.position, ec.transform.position);
			//Debug.Log (angle + "_____" + distance);
			if ((angle > 0 && distance < attDistance) || (angle <= 0 && distance < attDistance / 4))
			{
				ec.Hitted (animationItem, this);
			}
		}
	}

	void AttackAOEMessage (AnimationItem animationItem)
	{
		for (int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			float angle = FightMath.GetMultiplyVector (transform, ec.transform);
			float distance = FightMath.DistXZ (transform.position, ec.transform.position);
			//Debug.Log (angle + "_____" + distance);
			if (distance < attDistance)
			{
				ec.Hitted (animationItem, this);
			}
		}
	}

	private List<EnemyController> enemys = new List<EnemyController>();
	void assaultEvent (AnimationItem animationItem)
	{
		fpsController.moveDir (animationItem);
		//List<EnemyController> es = EnemyController.enemys;
		foreach (EnemyController ec in EnemyController.enemys)
		{
			Debug.Log (ec);
			enemys.Add (ec);
		}
	}

	void assaultDelegate (AnimationItem animationItem)//about spell
	{
		if (pro != PRO.FS)
		{
			if(animationItem.clip == Clip.Spell1)
			{

				//List<EnemyController> _enemys = EnemyController.enemys;
//				Debug.Log (EnemyController.enemys.Count + "___________" + enemys.Count);
				for (int i = 0; i < enemys.Count; i++)
				{
					EnemyController ec = enemys[i];
					float angle = FightMath.GetMultiplyVector (transform, ec.transform);
					float distance = FightMath.DistXZ (transform.position, ec.transform.position);
					//Debug.Log (angle + "_____" + distance);
					if ((angle > 0 && distance < attDistance) || (angle <= 0 && distance < attDistance / 4))
					{
						ec.Hitted (animationItem, this);
						//Debug.Log ("__haha__");
						enemys.RemoveAt (i);
						i--;
					}
				}
			}
		}
	}


	private bool IsIng = false;
	private float startAttTime = 0;
	public int maxAttackCount = 2;
	private int attckId = -1;
	private Clip clip;
	void Attack ()
	{
		if (IsIng == false)
		{
			attckId++;
			startAttTime = Time.time;
			IsIng = true;
			StartCoroutine (AttackQueue ());
			return;
		}
		if (clip != Clip.Null && Time.time - startAttTime > animationController.GetLength (clip) * 2 / 3)
		{
			startAttTime = Time.time;
			attckId++;
		}
	}
	
	IEnumerator AttackQueue ()
	{
		for (int i = (int)Clip.Attack1; i <= Mathf.Min (attckId, maxAttackCount - 1) + (int)Clip.Attack1; i++)
		{
			if (animationController.isFall || animationController.isHitted)
			{
				break;
			}
			clip = (Clip)i;
			animationController.Play (clip);
			yield return new WaitForSeconds (animationController.GetLength (clip));
		}
		clip = Clip.Null;
		attckId = -1;
		IsIng = false;
	}

	void Start () 
	{
		animationController = GetComponent<AnimationController>();
		animationController.attackEvent += attackEvent;
		animationController.attackAOEEvent += AttackAOEMessage;
		animationController.assaultEvent += assaultEvent;
		fpsController = GetComponent<FPSInputController>();
	}

	void Update () {
	
	}
}
