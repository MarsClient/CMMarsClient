using UnityEngine;
using System.Collections;

public class AnimationMessageManager : MonoBehaviour {

	private AiAnimation aiAnimation;

	void Awake ()
	{
		aiAnimation = transform.parent.GetComponent<AiAnimation>();
	}

	void IdleMessage (string info)
	{
		//Clip c = (Clip) int.Parse (info)
		//Debug.Log (info);
		aiAnimation.IdleMessage ();
	}

	void AttackMessage (string info)
	{
		string[] infos = info.Split (',');
		int c = int.Parse (infos[0]);
		int eventIndex = int.Parse (infos[1]);
		aiAnimation.AttackMessage (c, eventIndex);
	}

	void AnimationMove (string info)
	{
		string[] infos = info.Split (',');
		int c = int.Parse (infos[0]);
		int eventIndex = int.Parse (infos[1]);
		aiAnimation.AnimationMove (c, eventIndex);
	}

//	void AttackAOEMessage (string info)
//	{
//		int c = int.Parse (info);
//		aiAnimation.AttackMessage (c);
//	}

	/*void SpellAssault (int c)
	{
		aiAnimation.SpellAssault (c);
	}*/
}
