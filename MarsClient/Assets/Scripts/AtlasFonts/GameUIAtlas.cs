using UnityEngine;
using System.Collections;

public class GameUIAtlas : MonoBehaviour {

	private static GameUIAtlas m_instance;
	public static GameUIAtlas Instance
	{
		get
		{
			m_instance = ResourceLoadObj.SetResourceObjInstance<GameUIAtlas> (Constants.GAMEATLAS, m_instance);
			return m_instance;
		}
	}

	public UIFont normalFont;
}
