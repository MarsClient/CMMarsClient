using UnityEngine;
using System.Collections;
using System.ComponentModel;

public enum FxType
{
	NULL,
	Groud = 0,
	Parent,
}

public class GameEffect
{
	[DefaultValue (0)]
	public int id; //pro id;
	[DefaultValue (null)]
	public string assetbundle;
	[DefaultValue (0)]
	public int action;
	[DefaultValue (null)]
	public FxType fxType;
}
