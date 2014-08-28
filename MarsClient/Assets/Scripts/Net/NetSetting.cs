using UnityEngine;
using System.Collections;

public class NetSetting : MonoBehaviour {

	public string[] LoadingServerIps;
	public int index;

	public static NetSetting Instance;

	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public static string IP
	{
		get
		{
			int index = Mathf.Clamp (NetSetting.Instance.index, 0, NetSetting.Instance.LoadingServerIps.Length - 1);
			return NetSetting.Instance.LoadingServerIps[index];
		}
	}

	void Start ()
	{
		Application.runInBackground = true;
		PhotonClient.Instance.Start ();
	}

	void Update ()
	{

	}

	private void OnApplicationQuit ()
	{
		PhotonClient.Instance.PeerDiscount ();
	}
}
