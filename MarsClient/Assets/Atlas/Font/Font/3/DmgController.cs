using UnityEngine;
using System.Collections;

public class DmgController : MonoBehaviour {

	private UILabel label;

	public float range = 10;

	public void show ()
	{
		label = GetComponent <UILabel> ();
		Vector3 pos = transform.localPosition;
		label.color = Color.yellow;
		if (Random.Range (0, 2) == 0)
		{
			//label.color = Color.yellow;

			transform.localPosition = new Vector3 (Random.Range (-range + pos.x, range + pos.x), Random.Range (-range + pos.x, range + pos.x), 0);
		}
		else
		{
			//label.color = Color.yellow;
			transform.localScale = Vector3.one * 3;
			transform.localPosition = new Vector3 (Random.Range (-range + pos.x, range + pos.x), pos.y, 0);
			TweenScale.Begin (gameObject, 0.2f, Vector3.one);
		}

		label.text = Random.Range (1000, 10000).ToString ();

		Invoke ("StartAnt", 0.2f);
	}

	void StartAnt ()
	{
		Vector3 pos = new Vector3 (transform.localPosition.x, transform.localPosition.y + 100, transform.localPosition.z);

		TweenPosition.Begin (gameObject, 0.5f, pos);
		TweenAlpha.Begin (gameObject, 0.5f, 0);
		Invoke ("DestoryAnt", 0.5f);
	}

	void DestoryAnt ()
	{
		GameObject.Destroy (transform.parent.gameObject);
	}
}
