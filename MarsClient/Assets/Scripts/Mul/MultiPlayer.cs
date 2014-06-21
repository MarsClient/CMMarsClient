using UnityEngine;
using System.Collections;

public class MultiPlayer : MonoBehaviour {

	void Start ()
	{
		Transform target = ObjectPool.Instance.LoadObject ("Roles/" + Main.Instance.role.profession).transform;
		target.name = Main.Instance.role.accountId.ToString ();
		CameraController.instance.initialize (target);
	}

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

	}

	void ProcessResultSync (Bundle bundle)
	{
	}

	public void OnDestroy ()
	{
		ObjectPool.Instance.Clear ();
	}
}
