using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DC = DebugConsole;
using System.Security;
using ExitGames.Client.Photon;


public class NetEvents
{
	public delegate void ProcessResults (Bundle bundle);
	public delegate void ProcessResultSync (Bundle bundle);
}

public class PhotonClient : MonoBehaviour, IPhotonPeerListener {


	/*
	 *Events
	 */
	public static  NetEvents.ProcessResults ProcessResults;
	public static  NetEvents.ProcessResultSync ProcessResultSync;


	public static PhotonClient Instance;


	//LOAD SERVER ADDRESS
	public string LOAD_SERVER_ADDRESS = "localhost:5055";


	protected string ServerApplication = "EZServer";
	protected PhotonPeer peer;
	public bool ServerConnected {get; private set;}
	
	// Use this for initialization
	void Start () {

		Instance = this;
		Application.runInBackground = true;

		this.ServerConnected = false;

		DC.LogError ("Disconnected");

		this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
		this.Connect();
	}

	public virtual void Connect()
	{
		try
		{
			this.peer.Connect(this.LOAD_SERVER_ADDRESS, this.ServerApplication);
		}
		catch (SecurityException se)
		{
			this.DebugReturn(0, "Connection Failed. " + se.ToString());
		}
	}

	private bool isVal = false;
	// Update is called once per frame
	void Update () {
		this.peer.Service ();
		if (this.ServerConnected != isVal)
		{
			isVal = this.ServerConnected;
			if (isVal)
			{
				DC.Log ("Connected");
			}
			else
			{
				DC.LogError ("Disconnected");
			}
		}
	}

	public void OnStatusChanged (StatusCode statusCode)
	{
		this.DebugReturn(0, string.Format("PeerStatusCallback: {0}", statusCode));
		switch (statusCode)
		{
		case StatusCode.Connect:
			this.ServerConnected = true;
			break;
		case StatusCode.Disconnect:
			this.ServerConnected = false;
			break;
		}
	}

	public void SendServer (Command operationCode)
	{
		SendServer (operationCode, null);
	}

	public void SendServer (Command operationCode, object obj)
	{
		Dictionary<byte, object> parameter = null;
		if (obj != null)
		{
			parameter = new Dictionary<byte, object>();
			string json = JsonConvert.SerializeObject(obj);
			parameter.Add ((byte)operationCode, json);
			Debug.Log (json);
			DC.Log (json);
		}
		this.peer.OpCustom((byte)operationCode, parameter, true);
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		DC.Log (message);
		Debug.Log (message);
	}
	public void OnOperationResponse (OperationResponse operationResponse)
	{
		Bundle bundle = new Bundle ();
		string json = operationResponse[operationResponse.OperationCode].ToString();
		bundle = JsonConvert.DeserializeObject<Bundle>(json);
		bundle.cmd = (Command) operationResponse.OperationCode;//cmd
		Debug.Log (json);
		DC.LogWarning(json);
		CalledProcessResult (bundle);
	}

	public void OnEvent (EventData eventData)
	{
		if (eventData.Parameters.ContainsKey (eventData.Code) == false)
		{
			return;
		}
		Bundle bundle = new Bundle ();
		string json = eventData.Parameters[eventData.Code].ToString ();
		bundle = JsonConvert.DeserializeObject<Bundle>(json);
		bundle.eventCmd = (EventCommand) eventData.Code;
		Debug.Log (json);
		DC.LogWarning(json);
		CalledProcessEvent (bundle);
	}

	public void CalledProcessResult (Bundle bundle)
	{
		if (ProcessResults != null)
		{
			ProcessResults (bundle);
		}
	}

	public void CalledProcessEvent (Bundle bundle)
	{
		if (ProcessResultSync != null)
		{
			ProcessResultSync (bundle);
		}
	}
}
