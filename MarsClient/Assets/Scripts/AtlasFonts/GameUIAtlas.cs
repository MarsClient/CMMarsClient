using UnityEngine;
using System.Collections;

public class GameUIAtlas : MonoBehaviour {

	private static GameUIAtlas m_instance;
	public static GameUIAtlas Instance
	{
		get
		{
			if (m_instance == null)
			{
				GameObject resGo = Resources.Load (Constants.UI + "GameAtlas") as GameObject;
				m_instance = resGo.GetComponent<GameUIAtlas>();
			}
			return m_instance;
		}
	}

	public UIFont normalFont;
}
