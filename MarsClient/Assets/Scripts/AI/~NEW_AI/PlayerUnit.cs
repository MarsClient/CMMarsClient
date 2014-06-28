using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUnit : HitUnit {

	public static List<PlayerUnit> playersUnit = new List<PlayerUnit> ();

	private Role role;
	
	void Awake ()
	{
		m_ac = GetComponent <AiAnimation>();
		playersUnit.Add (this);
	}

	void Remove ()
	{
		playersUnit.Remove (this);
	}

	void LateUpdate ()
	{
		updateUIShow ();
	}

	public override void DataRefresh (object t)
	{
		role = t as Role;
		if (label != null && role != null)
		{
			label.bitmapFont = AssetLoader.Instance.normalFont;
			label.text = role.roleName;
		}
	}
}
