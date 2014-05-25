using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Troop
{
	private const float DMG_OFFSET = 0.1f;
	private const float DMG_ADD = 0.1f;


	[DefaultValue (null)]
	public string id;
	[DefaultValue (1)]
	public int level;
	[DefaultValue (null)]
	public string type;
	[DefaultValue (null)]
	public string assetbundle;
	[DefaultValue (null)]
	public string icon;
	[DefaultValue (null)]
	public string desc;
	[DefaultValue (null)]
	public string name;
	[DefaultValue (0F)]
	public float dmg;
	[DefaultValue (0F)]
	public float def;
	[DefaultValue (0F)]
	public float hpMax;
	[DefaultValue (0F)]
	public float hp;
	[DefaultValue (0F)]
	public float x
	{
		get{
			return 0;
		}
	}
	[DefaultValue (0F)]
	public float y
	{
		get{
			return 0;
		}
	}

	public float attDmg
	{
		get{
			float deage = (1 + DMG_ADD * (level - 1)) * dmg;
			return Random.Range (dmg * (1 - DMG_OFFSET), dmg * (1 + DMG_OFFSET));
		}
	}
}
