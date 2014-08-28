using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DC = DebugConsole;
using System.Security;
using ExitGames.Client.Photon;
using System.Threading;

public class NetClient : IPhotonPeerListener {

	public delegate void ProcessResults (Bundle bundle);
	public delegate void ProcessResultSync (Bundle bundle);
	public static  ProcessResults processResults;
	public static  ProcessResultSync processResultSync;

	private static NetClient mInstance;
	public static NetClient Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new NetClient();
			}
			return mInstance;
		}
	}

	private Thread oThread;

	private void StartThread ()
	{
		AbortThread ();
		oThread = new Thread(new ThreadStart (Update));
		oThread.Start();
	}

	private void AbortThread ()
	{
		if (oThread != null)
		{
			oThread.Abort ();
			oThread.Abort ();
		}
	}

	private string LoginServerApplication = "LoginServer";
	private string GameServerApplication = "MarsServer";

	public string load_address;//for see
	public string appserver;//for see

	protected PhotonPeer peer;
	public bool ServerConnected {get; private set;}

	/*Queue*/
	private Queue<string> COMMANDS = new Queue<string>();
	private Queue<string> COMMANDEVENTS = new Queue<string>();

	public virtual void Connect()
	{
		try
		{
			this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
			this.peer.Connect(load_address, appserver);
			StartThread ();
		}
		catch (SecurityException se)
		{
			this.DebugReturn(0, "Connection Failed. " + se.ToString());
		}
	}

	void Update () 
	{
		while (true)
		{
			if (peer != null)
			{
				this.peer.Service ();
				Thread.Sleep (15);
			}
		}
	}


#region IPhotonPeerListener API
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

	public void DebugReturn (DebugLevel level, string message)
	{
		Debug.Log (message);
		//NetLog (message);
	}
	public void OnOperationResponse (OperationResponse operationResponse)
	{
		if (operationResponse.Parameters.ContainsKey (operationResponse.OperationCode))
		{
			string json = operationResponse.Parameters[operationResponse.OperationCode].ToString();
			//Bundle bundle = JsonDeserialize (json);
			COMMANDS.Enqueue (json);
		}
	}

	public void OnEvent (EventData eventData)
	{
		if (eventData.Parameters.ContainsKey (eventData.Code))
		{
			string json = eventData.Parameters[eventData.Code].ToString ();
			//Bundle bundle = JsonDeserialize (json);
			COMMANDEVENTS.Enqueue (json);
		}
	}
#endregion

#region Get data from server API (contain debug)
	public void Start () 
	{
		LoadingLoginServer ();
	}
	
	public void LoadingLoginServer ()
	{
		this.ServerConnected = false;
		load_address = NetSetting.IP;
		appserver = LoginServerApplication;
		this.Connect();
	}
	
	public void LoadingGameServer (string ip)
	{
		this.ServerConnected = false;
		load_address = ip;
		appserver = GameServerApplication;
		this.Connect();
	}


	private bool isVal = false;
	public void UpdateQueue (NetRecv netRecv)
	{
		if (COMMANDS.Count > 0)
		{
			Bundle bundle =  JsonDeserialize (COMMANDS.Dequeue ());
			netRecv.ProcessResult (bundle);
			CalledProcessResult (bundle);
		}
		
		if (COMMANDEVENTS.Count > 0)
		{
			Bundle bundle = JsonDeserialize (COMMANDEVENTS.Dequeue ());
			netRecv.ProcessResult (bundle);
			CalledProcessEvent (bundle);
		}
		
		if (this.isVal != this.ServerConnected)
		{
			isVal = this.ServerConnected;
			if (isVal)
			{
				NetLog ("Net is connect");
			}
			else
			{
				NetLog ("Net is disconnect");
			}
			
		}
	}

	private Bundle JsonDeserialize (string json)
	{
		Bundle bundle = new Bundle ();
		json = NetEncrypt.Decrypt (json);
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
			if (processResultSync != null)
			{
				processResultSync (bundle);
			}
		}
	}

	public void PeerDiscount ()
	{
		AbortThread ();
		if (peer != null)
		{
			peer.Disconnect ();
			peer = null;
		}
	}

	/*
	 * for debug
	 **/
	private void NetLog (object message)
	{
		Debug.Log (message);
		DC.Log (message);
	}
#endregion

#region SendToServer API
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
				NetClient.Instance.NetLog (json);
			}
		}
		NetClient.Instance.peer.OpCustom((byte)operationCode, parameter, true);
	}
#endregion
}
