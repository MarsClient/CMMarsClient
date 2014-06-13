using UnityEngine;
using System.Collections;

public class MultiPlayer : MonoBehaviour {

	void OnEnable ()
	{
		PhotonClient.processResults += ProcessResults;
		PhotonClient.processResultSync += ProcessResultSync;
	}

	void OnDisable ()
	{
		PhotonClient.processResults -= ProcessResults;
		PhotonClient.processResultSync -= ProcessResultSync;
	}

	void ProcessResults (Bundle bundle)
	{
		if (bundle.cmd == Command.Login)
		{
			if (bundle.error == null)
			{
				GameObject myPlayer = ObjectPool.Instance.LoadObject ("Roles/ZS");
				CameraController.instance.initialize (myPlayer.transform);
				myPlayer.name = bundle.account.uniqueId.ToString ();
			}
		}
	}

	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.eventCmd == EventCommand.LobbyBroadcast)
		{
			if (bundle.error == null)
			{
				GameObject myPlayer = ObjectPool.Instance.LoadObject ("Roles/ZS");
				Destroy (myPlayer.GetComponent<AiInput>());
				myPlayer.GetComponent<AiMove> ().StartUpdate ();
//				Destroy (myPlayer.GetComponent<CharacterController>());
//				Destroy (myPlayer.GetComponent<FPSInputController>());
				myPlayer.name = bundle.account.uniqueId.ToString ();
			}
		}
		else if (bundle.eventCmd == EventCommand.InitAllPlayer)
		{
			if (bundle.error == null)
			{
				foreach (Player a in bundle.players)
				{
					GameObject myPlayer = ObjectPool.Instance.LoadObject ("Roles/ZS");
					Destroy (myPlayer.GetComponent<AiInput>());
					myPlayer.GetComponent<AiMove> ().StartUpdate ();
					myPlayer.transform.position = new Vector3 (a.x, 0, a.z);
					myPlayer.transform.forward = new Vector3 (a.xRo, 0, a.zRo);
					myPlayer.name = a.uniqueId.ToString ();
				}
			}
		}
		else if (bundle.eventCmd == EventCommand.UpdatePlayer)
		{
			if (bundle.error == null)
			{
				GameObject otherPlayer = GameObject.Find (bundle.player.uniqueId.ToString ());
				if (otherPlayer != null)
				{
					otherPlayer.transform.position = new Vector3 (bundle.player.x, 0, bundle.player.z);
					otherPlayer.transform.forward = new Vector3 (bundle.player.xRo, 0, bundle.player.zRo);
					otherPlayer.GetComponent<AiAnimation>().Play ((Clip)bundle.player.actionId);
				}
			}
		}
		else if (bundle.eventCmd == EventCommand.PlayerDisConnect)
		{
			if (bundle.error == null)
			{
				GameObject otherPlayer = GameObject.Find (bundle.player.uniqueId.ToString ());
				if (otherPlayer != null)
				{
					Destroy (otherPlayer);
				}
			}
		}
	}

	void AddPlayer (string id)
	{

	}
}
