using UnityEngine;
using System.Collections;

public abstract class HitUnit : MonoBehaviour {

	protected AiAnimation m_ac;
	public AiAnimation ac { get{ return m_ac; } }
	
	public Transform hitPos;

	public UILabel label;

	public void Hitted (AnimationInfo info, FrameEvent fe)
	{
//		if (info.clip == Clip.Spell1)
//		{
//			if (ac.isFall == false) { ac.Play (Clip.Fall); }
//			else return;
//		}
//		else if (ac.isFall == false)
//		{
//			ac.Play (Clip.Hit);
//		}
		//
		if (fe.isHit)
		{
			ac.Play (Clip.Hit);
		}
		else if (fe.isFall)
		{
			ac.Play (Clip.Fall);
		}

//		ObjectPool.Instance.LoadObject ("EF/EF0001", hitPos.position);
		ExtraEvent (info, fe);
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
			label.transform.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
		}
	}

	public virtual void ExtraEvent (AnimationInfo info, FrameEvent fe) {  }
	public virtual void DataRefresh (object t) { }
}
