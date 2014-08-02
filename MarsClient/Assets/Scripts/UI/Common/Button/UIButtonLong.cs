using UnityEngine;
using System.Collections;

public class UIButtonLong : MonoBehaviour {

	public float updateFrameRate = 0.03333f;

	public bool useUpdate
	{
		get
		{
			return (updateFrameRate == 0);
		}
	}

	public bool isPress;

	private const string MethodName = "UpdateInterval";
	void OnPress (bool isPress)
	{
		this.isPress = isPress;
		if (isPress == true)
		{
			BeginPressEvent ();
			if (useUpdate == false)
			{
				InvokeRepeating (MethodName, updateFrameRate, updateFrameRate);
			}
		}
		else
		{
			CancelInvoke (MethodName);
			EndPressEvent ();
		}
	}

	void Update ()
	{
		if (this.useUpdate == true)
		{
			if (isPress == true)
			{
				UpdateInterval ();
			}
		}
	}

	private void UpdateInterval ()
	{
		UpdatePressEvent ();
	}

	#region Tween
	public UISprite tweenTarget;
	public float duration;

	public Color colorHover = Color.white;
	public Color colorPress = Color.white;
	public bool isColorTween  =false;

	public Vector3 scaleHover = Vector3.one;
	public Vector3 scalePress = Vector3.one;
	public bool isScaleTween = false;

	public bool isLoop = false;

	void Begin ()
	{
		Reset ();
		if (isColorTween == true) TweenColor.Begin (tweenTarget.gameObject, duration, colorPress);
		if (isScaleTween == true) TweenScale.Begin (tweenTarget.gameObject, duration, scalePress);
	}

	void Reset ()
	{
		if (isColorTween == true) tweenTarget.color = colorHover;
		if (isScaleTween == true) tweenTarget.transform.localScale = scaleHover;
	}

	void End ()
	{
		if (isColorTween == true) TweenColor.Begin (tweenTarget.gameObject, duration, colorHover);
		if (isScaleTween == true) TweenScale.Begin (tweenTarget.gameObject, duration, scaleHover);
	}
	#endregion


	protected virtual void BeginPressEvent ()
	{
		Begin ();
	}
	protected virtual void UpdatePressEvent ()
	{
		if (isLoop == true)
		{
			Begin ();
		}
	}
	protected virtual void EndPressEvent ()
	{
		End ();
	}
}
