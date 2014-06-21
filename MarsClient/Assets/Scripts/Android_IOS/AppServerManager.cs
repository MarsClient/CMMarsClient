using UnityEngine;
using System.Collections;

public class AppServerManager : MonoBehaviour {

	public static AppServerManager Instance;

	void Awake () { if (Instance == null) { Instance = this; DontDestroyOnLoad (gameObject); } else if (Instance != this) Destroy (gameObject);}

#if UNITY_EDITOR

#elif UNITY_ANDROID

#endif
}
