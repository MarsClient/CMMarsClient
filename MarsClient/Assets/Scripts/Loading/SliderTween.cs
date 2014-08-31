using UnityEngine;
using System.Collections;

public class SliderTween : MonoBehaviour {

	private float curValue = 0;
	private float endValue = 0;
	private float lastTime = 0;
	private float interval = 0;

	private UISlider slider;
	private UILabel infoLabel;

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

	private void Init ()
	{
		if (slider == null)
		{
			slider = GetComponent<UISlider> ();
		}
		if (infoLabel == null)
		{
			infoLabel = GetComponentInChildren<UILabel>();
		}
	}

	private void Update ()
	{
		if (slider == null) return;
		interval = Time.time - lastTime;
		if (interval >= 1) return;
		curValue = Mathf.Lerp (curValue, endValue, interval);
		slider.value = curValue;
	}

#region Public Method
	public void SetSliderInfo (float sliderValue)
	{
		SetSliderInfo (sliderValue, null);
	}

	public void SetSliderInfo (float sliderValue, string info)
	{
		this.value = sliderValue;
		if (info != null)
		{
			infoLabel.text = info;
		}
		else
		{
			infoLabel.text = "Loading...";
		}
	}
#endregion
}
