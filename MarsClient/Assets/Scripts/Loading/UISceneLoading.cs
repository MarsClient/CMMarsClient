using UnityEngine;
using System.Collections;

public class UISceneLoading : MonoBehaviour {

	public static UISceneLoading instance;
	public static string loadName = "Splash";

	public UISlider slider;

	//public static void Load

	void Awake () { instance = this; }
	void OnDisable () { instance = null; }

	public static void LoadingScnens (string loadName)
	{
		UISceneLoading.loadName = loadName;
		Application.LoadLevel ("Loading");
	}


	private AsyncOperation async;
	IEnumerator  Start ()
	{
		if (UISceneLoading.loadName != null)
		{
			async = Application.LoadLevelAdditiveAsync (UISceneLoading.loadName);
			UISceneLoading.loadName = null;
			yield return async;
			yield return new WaitForSeconds (0.5f);
			Destroy (gameObject);
		}
	}

	void Update ()
	{
		if (async != null)
		{
			if (async.isDone == false)
			{
				slider.value = async.progress;
			}
			else
			{
				slider.value = 1.0f;
			}
		}
	}

	void OnGUI ()
	{
		if (GUILayout.Button ("Spell1"))
		{
			UISceneLoading.LoadingScnens ("Splash");
		}
		if (GUILayout.Button ("Spell2"))
		{

		}
	}
}
