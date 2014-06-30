﻿using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class GameBase
{
	[DefaultValue(0L)]
	public long id;
	[DefaultValue(null)]
	public string type;
	[DefaultValue(null)]
	public string name;
	[DefaultValue (null)]
	public string desc;
	[DefaultValue (null)]
	public string icon;
	[DefaultValue (null)]
	public string model;

}
