using UnityEngine;
using System.ComponentModel;
using System.Collections;

public class Spell
{
	private const float DMG_ADD = 0.2f;

	[DefaultValue(null)]
	public string id;
	[DefaultValue(null)]
	public string type;
	[DefaultValue(null)]
	public string assetbundle;
	[DefaultValue(null)]
	public string pro;
	[DefaultValue(null)]
	public string act;
	[DefaultValue(null)]
	public string icon;
	[DefaultValue(null)]
	public string desc;
	[DefaultValue(null)]
	public string name;
	[DefaultValue(0L)]
	public float _dmg;
	[DefaultValue(0L)]
	public float cd;
	[DefaultValue(0)]
	public int level;


	public float dmg
	{
		get
		{
			return _dmg + (level - 1) * DMG_ADD;//0.2 add;
		}
	}
}
