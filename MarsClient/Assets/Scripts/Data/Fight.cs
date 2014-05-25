using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Map
{
	[DefaultValue(null)]
	public string mapName;
	[DefaultValue (0F)]
	public float mapWidth;//2000
	[DefaultValue (0F)]
	public float mapHeigh;//400

	public Vector3 mapPos
	{
		get
		{
			return new Vector3 (0, 0, 1000);
		}
	}
}

public class Fight
{
	[DefaultValue(null)]
	public List<Troop> enemys;
	[DefaultValue(null)]
	public List<Troop> players;
	[DefaultValue (null)]
	public Map map;

	[DefaultValue (0F)]
	private float _hpMax;

	public float hpMax
	{
		get
		{
			if (_hpMax == 0)
			{
				foreach (Troop troop in enemys)
				{
					_hpMax += troop.hpMax;
				}
			}
			return _hpMax;
		}
	}
}
