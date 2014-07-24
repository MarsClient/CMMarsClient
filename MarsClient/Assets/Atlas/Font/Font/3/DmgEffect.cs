#define TEST_DMG

using UnityEngine;
using System.Collections;

public enum DmageEffect
{
	NORMAL,
	DOUBLE,
}

public class DmgEffect : MonoBehaviour {

	private const float PI = 3.1415926535898f;

	private float lastTime = 0;
	public float maxHeigh = 50;
	public float rateX = 0.05f;
	public float hiddenAngle = 120;
	public float spd = 10;

	private UILabel m_label;
	private float dir; //1 or -1;
	private float dirValue
	{
		get
		{
			return Mathf.Abs (dir);
		}
	}
	private float lastTweenTime = 0;
	private DmageEffect m_dmageEffect = DmageEffect.NORMAL;
	private Vector3 startScale;

	public void SetText (int dmg, DmageEffect dmageEffect = DmageEffect.NORMAL)
	{
		if (m_label == null)
		{
			m_label = GetComponentInChildren <UILabel>();
		}
		m_label.text = dmg.ToString ();
		m_label.alpha = 1.0f;
		m_dmageEffect = dmageEffect;
		lastTweenTime = Time.time;
		transform.localPosition = Vector3.zero;
		if (m_dmageEffect == DmageEffect.NORMAL)
		{
			startScale = Vector3.one;
			transform.localScale = startScale;
			lastTime = 0;
			dir = (Random.Range (0, 2) == 1) ? 1 : -1;
		}
		else if (m_dmageEffect == DmageEffect.DOUBLE)
		{
			startScale = Vector3.one * 3;
			transform.localScale = startScale;
			lastTime = Time.time;
		}
	}

	void OnEnable () 
	{
		//SetText (Random.Range (0, 100), DmageEffect.NORMAL);
		SetText (Random.Range (1000, 10000), (Random.Range (0, 2) == 1) ? DmageEffect.NORMAL : DmageEffect.DOUBLE);
	}
	
	void Update () 
	{
		if (m_dmageEffect == DmageEffect.NORMAL)
		{
			lastTime += Time.deltaTime * spd;
			float x = lastTime * dir;
			float angle = lastTime * rateX;
			float y = Mathf.Sin (angle) * maxHeigh;
			transform.localPosition = new Vector3 (x, y, 0);
			float final = angle * dirValue;
			float maxAngle = 2 * PI * hiddenAngle / 360;

			float time = maxAngle / (spd * rateX);
			if (m_label != null)
			{
				m_label.alpha = 1 - (Time.time - lastTweenTime) / time;
			}
		}
		else if (m_dmageEffect == DmageEffect.DOUBLE)
		{
			Vector3 scale = Vector3.Lerp (startScale, Vector3.one, (Time.time - lastTweenTime) * 10);
			transform.localScale = scale;
//			Debug.Log (scale);
			if (scale.x <= 1.0f)
			{
				m_label.alpha = Mathf.Lerp (1, 0, (Time.time - lastTime));
			}
		}
		if (m_label.alpha <= 0)
		{
			DestroyObj ();
		}
	}

	private PoolController m_pc;
	void DestroyObj ()
	{
		if (m_pc == null) m_pc = NGUITools.FindInParents<PoolController>(gameObject);
		m_pc.Release ();
		//Destroy (transform.parent.gameObject);
	}
}
