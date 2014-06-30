using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class GameSpell : GameBase {

	[DefaultValue(null)]
	public string func;
	[DefaultValue(null)]
	public string belong;
	[DefaultValue(0)]
	public int cd;
	[DefaultValue(null)]
	public string pro;
	[DefaultValue(0)]
	public int shoottype;
}
