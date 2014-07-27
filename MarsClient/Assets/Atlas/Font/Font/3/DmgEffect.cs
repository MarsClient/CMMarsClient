#define TEST_DMG

using UnityEngine;
using System.Collections;

public enum DmageEffect
{
	NORMAL,
	DOUBLE,
}

public enum DmageOpposite
{
	ENEMY,
	PLAYER,
}


public class DmgEffect : MonoBehaviour {

	private const float PI = 3.1415926535898f;

	public Color normalColor = Color.white;
	public Color doubleColor = Color.yellow;
	public Color hitPlayerColor = Color.yellow;

	public float maxHeigh = 50;
	public float rateX = 0.05f;
	public float hiddenAngle = 120;
	public float spd = 10;

	public UILabel m_label;
	private float lastTime = 0;
	private float dir; //1 or -1;
	private float dirValue
	{
		get
		{
			return Mathf.Abs (dir);
		}
	}
	private float lastTweenTime = 0;
	private float lastAlphaTime = 0;
	private DmageEffect m_dmageEffect = DmageEffect.NORMAL;
	private Vector3 startScale;
	private DmageOpposite m_dmageOpposite;
	private Color color
	{
		get
		{
			if (m_dmageOpposite == DmageOpposite.PLAYER)
			{
				return hitPlayerColor;
			}
			else
			{
				if (m_dmageEffect == DmageEffect.NORMAL)
				{
					return normalColor;
				}
				else if (m_dmageEffect == DmageEffect.DOUBLE)
				{
					return doubleColor;
				}
			}
			return Color.white;
		}
	}

	public void SetText (int dmg, DmageEffect dmageEffect = DmageEffect.NORMAL)
	{
		SetText (dmg, dmageEffect, DmageOpposite.ENEMY);
	}

	public void SetText (int dmg, DmageEffect dmageEffect, DmageOpposite dmageOpposite)
	{
		if (m_label == null)
		{
			m_label = GetComponentInChildren <UILabel>();
		}
		m_label.text = dmg.ToString ();
		m_label.alpha = 1.0f;
		m_dmageEffect = dmageEffect;
		m_dmageOpposite = dmageOpposite;
		lastTweenTime = Time.time;
		transform.localPosition = Vector3.zero;
		m_label.color = color;
		lastTime = 0;
		startScale = Vector3.one;
		transform.localScale = startScale;
		
		dir = (Random.Range (0, 2) == 1) ? 1 : -1;
		if (m_dmageEffect == DmageEffect.NORMAL)
		{

		}
		else if (m_dmageEffect == DmageEffect.DOUBLE)
		{
			startScale = Vector3.one;
			transform.localScale = startScale;
			lastAlphaTime = Time.time;
		}
	}

	void OnEnable () 
	{
		//SetText (Random.Range (0, 100), DmageEffect.NORMAL);
		//SetText (Random.Range (1000, 10000), (Random.Range (0, 2) == 1) ? DmageEffect.NORMAL : DmageEffect.DOUBLE);
	}
	
	void Update () 
	{
		if (m_dmageEffect == DmageEffect.NORMAL)
		{
			sinMove ();

		}
		else if (m_dmageEffect == DmageEffect.DOUBLE)
		{
			sinMove ();

			Vector3 b_Scale = Vector3.Lerp (startScale, Vector3.one * 2, (Time.time - lastTweenTime) * 5);
			transform.localScale = b_Scale;
			if (b_Scale.x >= 2.0f)
			{
				Vector3 s_Scale = Vector3.Lerp (startScale, Vector3.one, (Time.time - lastAlphaTime) * 2);
				transform.localScale = s_Scale;
			}
		}
		if (m_label.alpha <= 0)
		{
			ReleaseObj ();
		}
	}

	void sinMove ()
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

	private PoolController m_pc;
	void ReleaseObj ()
	{
		if (m_pc == null) m_pc = NGUITools.FindInParents<PoolController>(gameObject);
		m_pc.Release ();
		//Destroy (transform.parent.gameObject);
	}
}
