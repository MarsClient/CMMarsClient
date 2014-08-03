using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class GameMonster : GameBase
{
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
}
