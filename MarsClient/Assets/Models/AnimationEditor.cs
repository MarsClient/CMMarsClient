using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationEditor : MonoBehaviour {

	List<AnimationClip> ants = new List<AnimationClip>();
	// Use this for initialization
	void Start () {
		foreach (AnimationState state in animation)
		{

			ants.Add (state.clip);
		}

	}

	private Vector3 scrollPosition = Vector3.zero;
	void OnGUI () 
	{

		scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width / 3, Screen.height), scrollPosition, new Rect(0, 0, Screen.width, 10000), false, true);
		foreach (AnimationClip s in ants)
		{
			//Debug.Log (s.name);
			if (GUILayout.Button (s.name))
			{
				animation.CrossFade (s.name);
			}
		}
		GUI.EndScrollView ();
	}
}
