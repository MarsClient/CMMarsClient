using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct AnimationInfoCache
{
	public AnimationInfo info;
	public FrameEvent fe;
	public int dmg;
	public bool isDouble;
	public bool isDmg;
}

public abstract class HitUnit : MonoBehaviour {

	protected AiAnimation m_ac;
	public AiAnimation ac { get{ return m_ac; } }
	
	public Transform hitPos;
	public Transform bloodBar;

	public UILabel label;//for show unit name
	public UISlider slider;//for show blood;
	//public GameObject go;

	private Queue<AnimationInfoCache> caches = new Queue<AnimationInfoCache>();

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

	public void Hitted (AnimationInfoCache animationInfoCache/*AnimationInfo info, FrameEvent fe, int dmg, bool isDouble, bool isDmg = false*/)
	{
		caches.Enqueue (animationInfoCache);
		ExtraEvent (animationInfoCache.dmg);
	}

	protected void HitEffect ()
	{
		if (caches.Count == 0) return;

		AnimationInfoCache animationInfoCache = caches.Dequeue ();

		if (ac.dontMove == false)
		{
			if (animationInfoCache.fe.attackedClip == Clip.Null || animationInfoCache.fe.attackedClip == Clip.Idle) { if (m_ac.isRun == true) { ac.Play (Clip.Idle); } }
			else { ac.Play (animationInfoCache.fe.attackedClip); }
		}
		else
		{
			if (animationInfoCache.fe.attackedClip == Clip.Fall || animationInfoCache.fe.attackedClip == Clip.Hit)
			{
				//ac.Stop ();
				ac.Play (animationInfoCache.fe.attackedClip);
			}
		}
		
		PoolManager.Instance.LoadGameObject ("Bullets_10000", (GameObject go)=>
		                                     {
			go.transform.position = hitPos.position;
		}, Constants.EF);
		if (animationInfoCache.isDmg)
		{
			PoolManager.Instance.LoadGameObject ("DmgEffect",
			                                     (GameObject go)=>
			                                     {
				go.transform.position = hitPos.position;
				go.GetComponentInChildren<DmgEffect> ().SetText (animationInfoCache.dmg, animationInfoCache.isDouble ? DmageEffect.DOUBLE : DmageEffect.NORMAL);
			});
		}


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
			bloodBar.rotation = Quaternion.Euler (new Vector3 (-60, 0, 0));;
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

	public virtual void ExtraEvent (int dmg) {  }
	public virtual void DataRefresh (object t) { }
	public virtual void UnitDeath () {  }
}
