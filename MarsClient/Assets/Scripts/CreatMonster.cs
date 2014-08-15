using UnityEngine;
using System.Collections;

public class CreatMonster : MonoBehaviour {

	public string id;
	public string type;
//	public string name;
//	public string desc;
//	public string icon;
//	public string model;
//	public int value;
//	public int gold;
//	public int gem;
	public int level;
//	public float hp;
	public float hpMax;
	
	public float x;
	public float z;
//	public float xRo;
//	public float zRo;
//	public float action;
	public bool isBoss;

	/// <summary>
	/// The spells are boss's.
	/// e.g spell1  spell2...
	/// </summary>
	public string[] spells;
	//public string[] spells;
}
