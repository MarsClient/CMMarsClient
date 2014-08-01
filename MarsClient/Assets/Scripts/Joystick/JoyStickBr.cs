using UnityEngine;
using System.Collections;

public class JoyStickBr : MonoBehaviour {

	public float duration = 0.25f;
	public float startScale = 1.5f;
	public float startAlpha = 1.0f;

	public UISprite foreground;
	public UISprite background;

	void OnClick ()
	{

	}

	public void SetIcon (string spName)
	{
		background.spriteName = spName;
		foreground.spriteName = spName;
		StartTween ();
	}

	private void StartTween ()
	{
		background.alpha = startAlpha;
		background.transform.localScale = Vector3.one;

		TweenAlpha.Begin (background.gameObject, duration, 0);
		TweenScale.Begin (background.gameObject, duration, Vector3.one * startScale );

	}

	void OnGUI ()
	{
		if (GUILayout.Button ("xxxxxxxxxxxxxxxxxxxxxxx"))
		{
			StartTween ();
		}
	}
}
