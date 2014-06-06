using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUnit : HitUnit {

	public static List<PlayerUnit> playersUnit = new List<PlayerUnit> ();
	
	void Awake ()
	{
		m_ac = GetComponent <AiAnimation>();
		playersUnit.Add (this);
	}

	void Remove ()
	{
		playersUnit.Remove (this);
	}


}
