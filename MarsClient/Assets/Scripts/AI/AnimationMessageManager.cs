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

	void AttackMessage (int c)
	{
		aiAnimation.AttackMessage (c);
	}

	void AnimationMove (string info)
	{
		string[] infos = info.Split (',');
		//Debug.Log (infos[0] + "___" + infos[1]);
		int c = int.Parse (infos[0]);
		int eventIndex = int.Parse (infos[1]);
		aiAnimation.AnimationMove (c, eventIndex);
	}

	/*void AttackAOEMessage (int c)
	{
		aiAnimation.AttackAOEMessage (c);
	}

	void SpellAssault (int c)
	{
		aiAnimation.SpellAssault (c);
	}*/
}
