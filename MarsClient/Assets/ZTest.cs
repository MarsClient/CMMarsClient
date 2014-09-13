using UnityEngine;
using System.Collections;

public class ZTest : MonoBehaviour {

	public GameObject ItemPrefabs;
	void OnGUI ()
	{
		if (GUILayout.Button ("XXXXXXXXXXXXX"))
		{
			Instantiate (ItemPrefabs);
		}
	}
}
