using UnityEngine;
using System.Collections;

public class DmgManager : MonoBehaviour {

	public GameObject dmgPrefab;

	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 100, 100), "dmg"))
		{
			Transform go = NGUITools.AddChild (gameObject, dmgPrefab).transform;
//			go.localPosition = Vector3.zero;
			//go.GetComponent <DmgController>().show ();
		}
	}
}
