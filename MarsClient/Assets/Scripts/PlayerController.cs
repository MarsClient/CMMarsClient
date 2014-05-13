using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(FPSInputController))]
public class PlayerController : MonoBehaviour {

	public AnimationController animationController;

	void OnEnable ()
	{
		FPSInputController.inputController += inputController;
		FPSInputController.attackController += attackController;
	}

	void OnDisable ()
	{
		FPSInputController.inputController -= inputController;
		FPSInputController.attackController -= attackController;
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


	private bool IsIng = false;
	private float startAttTime = 0;
	public int maxAttackCount = 2;
	private int attckId = -1;
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
		if (Time.time - startAttTime > animationController.length / 2)
		{
			startAttTime = Time.time;
			attckId++;
		}
	}
	
	IEnumerator AttackQueue ()
	{
		for (int i = (int)Clip.Attack1; i <= Mathf.Min (attckId, maxAttackCount - 1) + (int)Clip.Attack1; i++)
		{
			animationController.Play ((Clip)i);
			yield return new WaitForSeconds (animationController.length);
		}
		attckId = -1;
		IsIng = false;
	}

	void Start () 
	{
		animationController = GetComponent<AnimationController>();
	}

	void Update () {
	
	}
}
