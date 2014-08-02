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
		new DialogContent()
			.SetMessage("game.fight.messgae", "ICC")
				.SetYesBtn ("game.dialog.yes")
				.SetNoBtn ("game.dialog.no")
				.SetDelegateBtn (EnterFight)
				.Show ();
		//
		//Debug.Log (gameNpc.name);
	}

	void EnterFight (bool isBy)
	{
		if (isBy)
		{
			Fight fight = new Fight ();
			fight.id = 100001;
			fight.type = "LV00001A";
			NetSend.SendEnterFight (fight);
			//UISceneLoading.LoadingScnens ("LV00001A", null, true);
		}
		//Debug.Log (isBy);
	}
}
