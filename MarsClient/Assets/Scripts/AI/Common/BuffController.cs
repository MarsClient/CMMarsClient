using UnityEngine;
using System.Collections;

public class BuffController : MonoBehaviour {

	private AiMove aiMove;
	private AiAnimation aiAnimation;
	private AIPath aiPath;

	void Start ()
	{
		aiMove = GetComponent<AiMove>();
		aiAnimation = GetComponent<AiAnimation>();
		aiPath = GetComponent<AIPath>();
	}

	#region Low Run
	/// <summary>
	/// low speed, run
	/// </summary>
	/// <param name="duration">Duration.</param>
	/// <param name="ratio">Ratio.</param>
	public void StartRunLow (float duration, float ratio)
	{
		StopCoroutine ("RunLowReset");
		//speed  and aniamtion
		//@speed
		aiMove.SetMoveSpeed (ratio);
		//@animation speed
		AnimationInfo info = aiAnimation.GetInfoByClip (Clip.Run);
		info.SetSpeaciedSpeed (ratio);

		ArrayList list = new ArrayList();
		list.Add (duration);
		list.Add (info);
		StartCoroutine ("RunLowReset", list);
	}

	IEnumerator RunLowReset (ArrayList list)
	{
		Debug.Log ("start" + Time.time);
		yield return new WaitForSeconds ((float)list[0]);
		Debug.Log ("end" + Time.time);
		//@speed
		aiMove.SetMoveSpeed ();
		//@animation speed
		AnimationInfo info = (AnimationInfo)list[1];
		info.SetSpeaciedSpeed ();
	}
	#endregion

//#if UNITY_EDITOR
	void OnGUI ()
	{
		if (GUILayout.Button ("LOW RUN"))
		{
			StartRunLow (3, 0.5F);
		}
	}
//#endif
}
