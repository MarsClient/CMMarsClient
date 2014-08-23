using UnityEngine;
using System.Collections;

public class IpSetting : MonoBehaviour {

	public string[] LoadingServerIps;
	public int index;

	public static IpSetting Instance;

	void Awake ()
	{
		Instance = this;
	}

	public static string IP
	{
		get
		{
			int index = Mathf.Clamp (IpSetting.Instance.index, 0, IpSetting.Instance.LoadingServerIps.Length - 1);
			return IpSetting.Instance.LoadingServerIps[index];
		}
	}
}
