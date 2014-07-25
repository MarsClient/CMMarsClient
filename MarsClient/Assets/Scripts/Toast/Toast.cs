using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toast : MonoBehaviour {

	private static Toast instance;
	void Awake () {if (instance == null) { instance = this; DontDestroyOnLoad (gameObject); } else if (instance != this) Destroy (gameObject); }

	private GameObject[] allItems = new GameObject[5];
	private Queue<string> queue = new Queue<string>();

	public GameObject parent;
	public GameObject prefab;
	public int maxCount = 5;
	public float distance = 25;
	public float lastTime = 2.0f;

	private float tweenDuration = 1.0f;

	private int disableNum 
	{ 
		get 
		{ 
			int i = 0;
			foreach (GameObject go in allItems)
			{
				if (go == null) {continue;}
				if (go != null && go.activeSelf == true ) { i++; }
			}
			return i;
		}
	}

	public static void ShowNormalText (string text)
	{
		instance.ShowText (text);
	}

	private void ShowText (string text)
	{
		ShowText (text, true);
	}

	private void ShowText (string text, bool isQueue)
	{
		if (isQueue) { queue.Enqueue (text); }
		int count = disableNum;
		if (count < maxCount)
		{
			if (isQueue) { string message = queue.Dequeue (); }
			StartCoroutine (delayTween (AddChild (text)));
		}
	}

	IEnumerator delayTween (GameObject go)
	{
		yield return new WaitForSeconds (lastTime);

		TweenAlpha.Begin (go, tweenDuration, 0);
		yield return new WaitForSeconds (tweenDuration);
		go.SetActive (false);
		if (queue.Count > 0)
		{
			string message = queue.Dequeue (); 

			ShowText (message, false);
		}
	}

	GameObject AddChild (string text)
	{
		for (int j = 0; j < maxCount; j++)
		{
			if (allItems[j] != null && allItems[j].activeSelf == true)
			{
				Vector3 pos = allItems[j].transform.localPosition;
				allItems[j].transform.localPosition = new Vector3 (pos.x, Mathf.Min (pos.y + distance, distance * (maxCount - 1)));
			}
		}


		GameObject go = null;
		for (int i = 0; i < maxCount; i++)
		{
			bool isNeedSet = (allItems[i] == null || allItems[i].activeSelf == false);
			if (allItems[i] == null)
			{
				go = NGUITools.AddChild (parent, prefab);
				allItems[i] = go;
				allItems[i].SetActive (true);
			}
			else if (allItems[i].activeSelf == false)
			{
				go = allItems[i];
				allItems[i].SetActive (true);
			}
			if (isNeedSet)
			{
				UILabel label = allItems[i].GetComponentInChildren<UILabel> ();
				//allItems[i].transform.localPosition = new Vector3 (0, distance * i, 0);
				allItems[i].transform.localPosition = Vector3.zero;
				label.alpha = 0;
				TweenAlpha.Begin (allItems[i], tweenDuration / 2, 1.0f);
				label.text = text;
				break;
			}
		}
		return go;
	}

	int i = 0;
	void OnGUI ()
	{
		if (GUILayout.Button ("hahahahhahahah"))
		{
			string s = "hahahahhahahahhahahahhahahahhahahahhahahah" + i++.ToString ();
			ShowText (s);
			//Debug.Log (s);
		}
		if (GUILayout.Button ("reset"))
		{
			i = 0;
		}//
//		if (GUILayout.Button ("hahahahhahahah"))
//		{
//			new DialogContent ()
//				.SetMessage ("test")
//					.SetYesBtn ("Yes")
//					.SetNoBtn ("No")
//					.SetDelegateBtn (DialogBtnEvent)
//					.Show ();
//		}
//
//		if (GUILayout.Button ("hahahahhahahah"))
//		{
//			new DialogContent ()
//				.SetMessage ("test")
//					.SetNoBtn ("Have a Wait....")
//					.SetDelegateBtn (DialogBtnEvent)
//					.ShowWaiting ();
//		}
	}
//
//	void DialogBtnEvent (bool yes)
//	{
//		Debug.Log (yes);
//	}
}
