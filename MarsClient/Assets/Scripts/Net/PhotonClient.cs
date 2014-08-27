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

	void Awake () 
	{ 
		if (Instance == null) 
		{ 
			Instance = this; 
			DontDestroyOnLoad (gameObject);
		}
		else if (Instance != this) 
		{
			Destroy (gameObject);
		}
	}

	private string LoginServerApplication = "LoginServer";
	private string GameServerApplication = "MarsServer";

	public string load_address;//for see
	public string appserver;//for see

	protected PhotonPeer peer;
	public bool ServerConnected {get; private set;}

	/*Queue*/
	private Queue<Bundle> COMMANDS = new Queue<Bundle>();
	private Queue<Bundle> COMMANDEVENTS = new Queue<Bundle>();

	private NetRecv netRecv;

	public void Start () {
		netRecv = GetComponent <NetRecv>();
		Application.runInBackground = true;
		this.ServerConnected = false;
		LoadingLoginServer ();
	}

	public void LoadingLoginServer ()
	{
		load_address = IpSetting.IP;
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
		if (peer != null)
		{
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

			UpdateQueue ();
		}
	}

	void UpdateQueue ()
	{
		if (COMMANDS.Count > 0)
		{
			Bundle bundle = COMMANDS.Dequeue ();
			CalledProcessResult (bundle);
		}

		if (COMMANDEVENTS.Count > 0)
		{
			Bundle bundle = COMMANDEVENTS.Dequeue ();
			CalledProcessEvent (bundle);
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

			Bundle bundle = new Bundle ();
			bundle.error = new Error ();
			bundle.error.message = "game.server.net.error";
			bundle.cmd = Command.NetError;
			CalledProcessResult (bundle);
			this.ServerConnected = false;
			break;
		}
	}

	public void PeerDiscount ()
	{
		peer.Disconnect ();
		peer = null;
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

			/*operationCode contain update dont show denug*/
			if (operationCode != Command.UpdatePlayer && operationCode != Command.TeamUpdate)
			{
				PhotonClient.Instance.NetLog (json);
			}
		}
		PhotonClient.Instance.peer.OpCustom((byte)operationCode, parameter, true);
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		NetLog (message);
	}
	public void OnOperationResponse (OperationResponse operationResponse)
	{
		if (operationResponse.Parameters.ContainsKey (operationResponse.OperationCode))
		{
			string json = operationResponse.Parameters[operationResponse.OperationCode].ToString();
			Bundle bundle = JsonDeserialize (json);
			COMMANDS.Enqueue (bundle);
		}
	}

	public void OnEvent (EventData eventData)
	{
		if (eventData.Parameters.ContainsKey (eventData.Code))
		{
			string json = eventData.Parameters[eventData.Code].ToString ();
			Bundle bundle = JsonDeserialize (json);

			COMMANDEVENTS.Enqueue (bundle);
		}
	}

	private Bundle JsonDeserialize (string json)
	{
		Bundle bundle = new Bundle ();
		json = CSharpEncrypt.Decrypt (json);
		bundle = JsonConvert.DeserializeObject<Bundle>(json);

		/*operationCode contain update dont show denug*/
		if (bundle.cmd != Command.UpdatePlayer && bundle.cmd != Command.TeamUpdate)
		{
			NetLog (json);
		}
		return bundle;
	}

	private void CalledProcessResult (Bundle bundle)
	{
		if (bundle != null)
		{
			netRecv.ProcessResult (bundle);
			if (processResults != null)
			{
				processResults (bundle);
			}
		}
	}

	private void CalledProcessEvent (Bundle bundle)
	{
		if (bundle != null)
		{
			netRecv.ProcessResultSync (bundle);
			if (processResultSync != null)
			{
				processResultSync (bundle);
			}
		}
	}

	private void OnApplicationQuit ()
	{
		PeerDiscount ();
	}

	/*
	 * for debug
	 **/
	private void NetLog (object message)
	{
		Debug.Log (message);
		DC.Log (message);
	}
}
