using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcManager : MonoBehaviour {

	public static NpcManager instance;

	private Dictionary<long, NpcController> npcs = new Dictionary<long, NpcController> ();

	void Awake ()
	{
		instance = this;
	}

	void OnDestroy()
	{
		npcs.Clear ();
		instance = null;
	}

	public void AddNpcController(NpcController nc)
	{
		if (npcs.ContainsKey (nc.gameNpc.id) == false || npcs[nc.gameNpc.id] == null )
		{
			npcs[nc.gameNpc.id] = nc;
			//npcs.Add (nc.gameNpc.id, nc);
		}
	}
}
