using UnityEngine;
using System.Collections;

public abstract class HitUnit : MonoBehaviour {

	protected AiAnimation m_ac;
	public AiAnimation ac { get{ return m_ac; } }
	
	public Transform hitPos;
	public Transform bloodBar;

	public UILabel label;//for show unit name
	public UISlider slider;//for show blood;
	//public GameObject go;

	public void Awake ()
	{
		m_ac = GetComponent <AiAnimation>();
		m_ac.unitDeath = UnitDeath;
	}

	public virtual void InitUI (GameBase gb)
	{
		if (bloodBar != null)
		{
			GameObject resGo = Resources.Load ("BloodBarBar") as GameObject;
			GameObject go = NGUITools.AddChild (bloodBar.gameObject, resGo);
			slider = go.GetComponentInChildren<UISlider>();
		}
		slider.value = 1.0f;
		UILabel lvLabel = slider.GetComponentInChildren<UILabel>();
		lvLabel.text = gb.level.ToString ();
	}

	private GameObject dmgParent;
	private GameObject dmgPrefab;

	public void Hitted (AnimationInfo info, FrameEvent fe, int dmg, bool isDouble, bool isDmg = false)
	{
		if (ac.isFall == false)
		{
			if (fe.attackedClip == Clip.Null || fe.attackedClip == Clip.Idle) { if (m_ac.isRun == true) { ac.Play (Clip.Idle); } }
			else { ac.Play (fe.attackedClip); }
		}
		else  { return; }


		Transform ef = PoolManager.Instance.LoadGameObject ("EF0001", null).transform;
		ef.transform.position = hitPos.position;
		if (isDmg)
		{
			Transform _go = PoolManager.Instance.LoadGameObject ("DmgEffect").transform;
			_go.position = hitPos.position;
			_go.GetComponentInChildren<DmgEffect> ().SetText (dmg, isDouble ? DmageEffect.DOUBLE : DmageEffect.NORMAL);
		}


		ExtraEvent (info, fe, dmg);
		//hit color
		CancelInvoke ("ResetColor");
		SetColor (1.0f);
		Invoke ("ResetColor", 0.1f);
	}
	
	private void ResetColor ()
	{
		SetColor (0);
	}
	
	private void SetColor (float a)
	{
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
				Color c = m.color;
				c.a = a;
				m.color = c;
			}
		}
	}

	public void updateUIShow ()
	{
		if (label != null)
		{
			FightMath.setRota (label.transform);//.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
		}
		if (bloodBar != null)
		{
			bloodBar.rotation = Quaternion.Euler (Vector3.zero);
		}
	}

	public void AlphaTween ()
	{
		float delay = 0.5f;
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
				string shaderName = "Transparent/Diffuse";
				if (m.shader.name != shaderName)
				{
					m.shader = Shader.Find (shaderName);
					Color  sc = m.color;
					sc.a = 1;
					m.color = sc;

					Color ec = m.color;
					ec.a = 0;
					TweenColor.Begin (smr.gameObject, delay, ec);
				}
			}
		}
		Invoke ("HiddenMe", delay);
	}

	void HiddenMe () { gameObject.SetActive (false); }

	public virtual void ExtraEvent (AnimationInfo info, FrameEvent fe, int dmg) {  }
	public virtual void DataRefresh (object t) { }
	public virtual void UnitDeath () {  }
}
