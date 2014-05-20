using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(FPSInputController))]
public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public AnimationController animationController;
	[HideInInspector]
	public FPSInputController fpsController;

	void OnEnable ()
	{
		FPSInputController.inputController += inputController;
		FPSInputController.attackController += attackController;
	}

	void OnDisable ()
	{
		FPSInputController.inputController -= inputController;
		FPSInputController.attackController -= attackController;
		animationController.attackEvent -= attackEvent;
	}

	void inputController (FPSInputController fpsController)
	{
		fpsController.canControl = !animationController.doNotMove;
		if (fpsController.canControl == false)
		{
			return;
		}
		//Move
		if (fpsController.directionVector.x != 0 || fpsController.directionVector.z != 0)
		{
			animationController.Play (Clip.Run);
		}
		else
		{
			animationController.Play (Clip.Idle);
		}
	}

	void attackController (FPSInputController fpsController)
	{
		Attack ();
	}

	void attackEvent (AnimationItem animationItem)
	{
		fpsController.moveDir (animationItem.actionMove);
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
		fpsController = GetComponent<FPSInputController>();
	}

	void Update () {
	
	}
}
