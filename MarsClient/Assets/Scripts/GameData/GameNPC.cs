using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class GameNPC : GameBase {

	[DefaultValue(0)]
	public int talkNum;
	[DefaultValue(0)]
	public int region;
	[DefaultValue(0f)]
	public float x;
	[DefaultValue(0f)]
	public float z;
	[DefaultValue(0f)]
	public float roY;
}
