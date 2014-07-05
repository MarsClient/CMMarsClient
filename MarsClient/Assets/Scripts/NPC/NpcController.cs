using UnityEngine;
using System.Collections;

public class NpcController : MonoBehaviour {

	public GameNPC gameNpc;

	public UILabel label;
//	void Statrt ()
//	{
//		if (label != null)
//		{
//			label.transform.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
//		}
//	}

	public void Refresh (GameNPC _gameNpc)
	{
		this.gameNpc = _gameNpc;
		transform.position = new Vector3 (_gameNpc.x, 0, _gameNpc.z);
		transform.rotation = Quaternion.Euler (new Vector3 (0, _gameNpc.roY, 0));
		if (label != null)
		{
			//Debug.Log (_gameNpc.name);
			FightMath.setRota (label.transform);//.rotation = Quaternion.Euler (new Vector3 (60, 180, 0));
		}
		label.bitmapFont = AssetLoader.Instance.normalFont;
		label.text = GameData.Instance.getLocalString (_gameNpc.name);

		NpcManager.instance.AddNpcController (this);
	}

	void OnClick ()
	{
		UISceneLoading.LoadingScnens ("LV00001A", null, true);
		Debug.Log (gameNpc.name);
	}
}
