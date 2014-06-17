using UnityEngine;
using System.Collections;

public class LoadGC : MonoBehaviour {

	public string currentLanguage = "Chinese";
	public TextAsset [] textAssets;

	public void Start ()
	{
		ModifyLanuage (currentLanguage);
	}

	public void ModifyLanuage (string str)
	{
		foreach (TextAsset ta in textAssets)
		{
			if (str == ta.name )
				Localization.Load (ta);
		}
	}
}
