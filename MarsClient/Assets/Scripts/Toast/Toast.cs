using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toast : MonoBehaviour {

	private static Toast instance;
	void Awake () {if (instance == null) { instance = this; DontDestroyOnLoad (gameObject); } else if (instance != this) Destroy (gameObject); }

	private GameObject[] allItems = new GameObject[5];// = new List<GameObject> (5);
	private Queue<string> queue = new Queue<string>();

	public GameObject parent;
	public GameObject prefab;
	public int maxCount = 5;
	public float distance = 25;
	public float lastTime = 2.0f;

	private float tweenDuration = 1.0f;

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
		int count = parent.transform.childCount;//GetComponentsInChildren <UILabel>().Length;//allItems.Count;
		//Debug.Log (count);
		if (count < maxCount)
		{
			//Debug.Log (text);
			if (isQueue) { string message = queue.Dequeue (); /*Debug.LogError (message)*/; }
			StartCoroutine (delayTween (AddChild (text)));
		}
	}

	IEnumerator delayTween (GameObject go)
	{
		yield return new WaitForSeconds (lastTime);

		TweenAlpha.Begin (go, tweenDuration, 0).ignoreTimeScale = false;
		yield return new WaitForSeconds (tweenDuration);
		DestroyImmediate (go);
		if (queue.Count > 0)
		{
			string message = queue.Dequeue (); 
			//Debug.LogError (message);
			ShowText (message, false);
		}
		//StartCoroutine (delayDestory (go));
	}

	GameObject AddChild (string text)
	{
		GameObject go = null;
		for (int i = 0; i < maxCount; i++)
		{
			//Debug.Log ("i = " + i);
			bool isNeedSet = (allItems[i] == null || allItems[i].activeSelf == false);
			if (allItems[i] == null)
			{
				go = NGUITools.AddChild (parent, prefab);
				allItems[i] = go;
				allItems[i].SetActive (true);
			}
			else if (allItems[i].activeSelf == false)
			{
				allItems[i].SetActive (true);
			}
			if (isNeedSet)
			{
				UILabel label = allItems[i].GetComponentInChildren<UILabel> ();
				allItems[i].transform.localPosition = new Vector3 (0, distance * i, 0);
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
		//Debug.LogError (parent.transform.childCount);
		if (GUILayout.Button ("hahahahhahahah"))
		{
			string s = "test" + i++.ToString ();
			ShowText (s);
			//Debug.Log (s);
		}
		if (GUILayout.Button ("reset"))
		{
			i = 0;
		}
	}
}
