using UnityEngine;
using System.Collections;

public class SliderTween : MonoBehaviour {

	private float curValue = 0;
	private float endValue = 0;
	private float lastTime = 0;

	public float value
	{
		set 
		{
			Init ();
			endValue = value;
			slider.value = curValue;
			lastTime = Time.time;
		}
		get 
		{
			return slider.value;
		}
	}

	private UISlider slider;

	void Init ()
	{
		if (slider == null)
		{
			slider = GetComponent<UISlider> ();
		}
	}

	private float interval;
	void Update ()
	{
		if (slider == null) return;
		interval = Time.time - lastTime;
		if (interval >= 1) return;
		curValue = Mathf.Lerp (curValue, endValue, interval);
		slider.value = curValue;
	}
}
