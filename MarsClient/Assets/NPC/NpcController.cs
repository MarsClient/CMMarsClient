using UnityEngine;
using System.Collections;

public class NpcController : MonoBehaviour {

	public UILabel label;
	void Statrt ()
	{
		if (label != null)
		{
			label.transform.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
		}
	}
}
