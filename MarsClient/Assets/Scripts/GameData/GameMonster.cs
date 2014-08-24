using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class GameMonster : GameBase
{
	[DefaultValue (null)]
	public string id;
	[DefaultValue (0F)]
	public float x;
	[DefaultValue (0F)]
	public float z;
	[DefaultValue (0F)]
	public float xRo;
	[DefaultValue (0F)]
	public float zRo;
	[DefaultValue (0F)]
	public float action;
	[DefaultValue (false)]
	public bool isBoss;

	[DefaultValue(0)]
	public int state;
	
	[DefaultValue(0F)]
	public float target_x;
	[DefaultValue(0F)]
	public float target_y;
	[DefaultValue(0F)]
	public float target_z;
}
