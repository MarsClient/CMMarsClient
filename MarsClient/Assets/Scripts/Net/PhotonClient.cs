using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DC = DebugConsole;
using System.Security;
using ExitGames.Client.Photon;

public class PhotonClient : MonoBehaviour, IPhotonPeerListener {

	public delegate void ProcessResults (Bundle bundle);
	public delegate void ProcessResultSync (Bundle bundle);
	public static  ProcessResults processResults;
	public static  ProcessResultSync processResultSync;


	public static PhotonClient Instance;

	//LOAD SERVER ADDRESS
	public string LOAD_LOGIN_SERVER_ADDRESS = "localhost:5055";
	private string LoginServerApplication = "LoginServer";
	//private string LoginServerApplication = "MarsServer";
	//Game Server
	public string LOAD_GAME_ADDRESS;
	private string GameServerApplication = "MarsServer";

	public string load_address;
	public string appserver;

	protected PhotonPeer peer;
	public bool ServerConnected {get; private set;}

	NetRecv netRecv;

	void Start () {

		Instance = this;
		Application.runInBackground = true;
		this.ServerConnected = false;
		DC.LogError ("Disconnected");
		LoadingLoginServer ();
	}

	public void LoadingLoginServer ()
	{
		load_address = LOAD_LOGIN_SERVER_ADDRESS;
		appserver = LoginServerApplication;
		this.Connect();
	}

	public void LoadingGameServer (string ip)
	{
		load_address = ip;
		appserver = GameServerApplication;
		this.Connect();
	}

	public virtual void Connect()
	{
		try
		{
			this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
			this.peer.Connect(load_address, appserver);
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
			if (netRecv == null) netRecv = GetComponent<NetRecv>();
			Bundle bundle = new Bundle ();
			bundle.error = new Error ();
			bundle.error.message = "game.server.net.error";
			bundle.cmd = Command.NetError;
			netRecv.ProcessResult (bundle);
			this.ServerConnected = false;
			break;
		}
	}

	public static void SendServer (Command operationCode)
	{
		SendServer (operationCode, null);
	}

	public static void SendServer (Command operationCode, object obj)
	{
		Dictionary<byte, object> parameter = null;
		if (obj != null)
		{
			parameter = new Dictionary<byte, object>();
			string json = JsonConvert.SerializeObject(obj) + "/" + operationCode.ToString ();
			parameter.Add ((byte)operationCode, json);
			Debug.Log (json);
			DC.Log (json);
		}
		PhotonClient.Instance.peer.OpCustom((byte)operationCode, parameter, true);
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		DC.Log (message);
		Debug.Log (message);
	}
	public void OnOperationResponse (OperationResponse operationResponse)
	{
		if (operationResponse.Parameters.ContainsKey (operationResponse.OperationCode))
		{
			string json = operationResponse.Parameters[operationResponse.OperationCode].ToString();
			Bundle bundle = JsonDeserialize (json);
			if (netRecv == null) netRecv = GetComponent<NetRecv>();
			netRecv.ProcessResult (bundle);
			CalledProcessResult (bundle);
		}
	}

	public void OnEvent (EventData eventData)
	{
		if (eventData.Parameters.ContainsKey (eventData.Code))
		{
			string json = eventData.Parameters[eventData.Code].ToString ();
			Bundle bundle = JsonDeserialize (json);
			if (netRecv == null) netRecv = GetComponent<NetRecv>();
			netRecv.ProcessResultSync (bundle);
			CalledProcessEvent (bundle);
		}
	}

	private Bundle JsonDeserialize (string json)
	{
		Bundle bundle = new Bundle ();
		json = CSharpEncrypt.Decrypt (json);
		bundle = JsonConvert.DeserializeObject<Bundle>(json);
		Debug.Log (json);
		DC.LogWarning(json);
		return bundle;
	}

	private void CalledProcessResult (Bundle bundle)
	{
		if (processResults != null && bundle != null)
		{
			processResults (bundle);
		}
	}

	private void CalledProcessEvent (Bundle bundle)
	{
		if (processResultSync != null && bundle != null)
		{
			processResultSync (bundle);
		}
	}

//	private void OnApplicationQuit ()
//	{
//		//Update ();
//		NetSend.SendAbortDiscount ();
//		//Update ();
//	}
}
