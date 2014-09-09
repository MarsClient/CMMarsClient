using UnityEngine;
using System.Collections;
using GmUpdate;

public class GameUpdateInterface : MonoBehaviour
{
	public static GameUpdateInterface instance;

	public SliderTween slider;

	void Awake ()
	{
		instance = this;
	}
}
