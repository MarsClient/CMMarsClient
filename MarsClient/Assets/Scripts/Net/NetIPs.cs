using UnityEngine;
using System.Collections;

public class NetIPs : MonoBehaviour {

	public string[] LoadingServerIps;
	public int index;

	public static NetIPs Instance;

	void Awake ()
	{
		Instance = this;
	}

	public static string IP
	{
		get
		{
			int index = Mathf.Clamp (NetIPs.Instance.index, 0, NetIPs.Instance.LoadingServerIps.Length - 1);
			return NetIPs.Instance.LoadingServerIps[index];
		}
	}
}
