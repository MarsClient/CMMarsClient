using UnityEngine;
using System.Collections;

/// <summary>
/// Decreases a light's intensity over time
/// </summary>

[RequireComponent(typeof(Light))]
public class CFX_LightIntensityFade : MonoBehaviour
{
	/// <summary>
	/// Duration of the effect.
	/// </summary>
	public float duration = 1.0f;
	
	/// <summary>
	/// Delay of the effect.
	/// </summary>
	public float delay = 0.0f;
	
	/// <summary>
	/// Final intensity of the light.
	/// </summary>
	public float finalIntensity = 0.0f;
	
	/// <summary>
	/// Base intensity, automatically taken from light parameters.
	/// </summary>
	private float baseIntensity;
	
	/// <summary>
	/// If <c>true</c>, light will destructs itself on completion of the effect
	/// </summary>
	public bool autodestruct;
	
	private float lifetime = 0.0f;
	
	void Start()
	{
		baseIntensity = light.intensity;
		if(delay > 0) light.enabled = false;
	}
	
	void Update ()
	{
		if(delay > 0)
		{
			delay -= Time.deltaTime;
			if(delay <= 0)
			{
				light.enabled = true;
			}
			return;
		}
		
		if(lifetime/duration < 1.0f)
		{
			light.intensity = Mathf.Lerp(baseIntensity, finalIntensity, lifetime/duration);
			lifetime += Time.deltaTime;
		}
		else if(autodestruct)
		{
			GameObject.Destroy(this.gameObject);
		}
		
	}
}
