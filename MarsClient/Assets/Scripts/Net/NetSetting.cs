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

	private NetRecv netRecv;

	/// <summary>
	/// index 0 is net disconnect message
	/// index 1 is todo
	/// </summary>
	public string[] someJson;

	void Start ()
	{
		netRecv = GetComponent<NetRecv>();
		Application.runInBackground = true;
		NetClient.Instance.Start (netRecv, someJson);
	}

	void Update ()
	{
		NetClient.Instance.UpdateQueue ();
	}

	private void OnApplicationQuit ()
	{
		NetClient.Instance.PeerDiscount ();
	}
}
