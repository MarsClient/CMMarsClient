using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;

namespace GmUpdate
{
	public class GameUpdate
	{
		public static readonly GameUpdate instance = new GameUpdate();

		private List<GameUpdateListeners> gameUpdateListeners = new List<GameUpdateListeners>();

		private Thread apkThread;//for android
		private Thread resThread;// android & IOS
		private Thread unZipThread;// android & IOS

#region ResDownload
		public void StartResDownload ()
		{
			StartThread (ref resThread, new ThreadStart (Run));
		}

		public void AbortResDownload ()
		{
			AbortThread (ref resThread);
		}
#endregion 


#region Thread Func
		private void Run ()
		{
			DownloadFile (Common.URL + Common.ZIP_NAME, Common.STORE_PATH);
		}
#endregion

#region Listeners
		public void AddElementListener (GameUpdateListeners listeners)
		{
			gameUpdateListeners.Add (listeners);
		}

		public void RemoveElementListener (GameUpdateListeners listeners)
		{
			gameUpdateListeners.Remove (listeners);
		}


		void DownloadFile (float progress)
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.DownloadFile (progress);
			}
		}
		void DownloadFileFinish ()
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.DownloadFileFinish ();
			}
		}
		void UnZipFile ()
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.UnZipFile ();
			}
		}
		void UnZipFileFinish ()
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.UnZipFileFinish ();
			}
		}
#endregion


		private void StartThread (ref Thread thread, ThreadStart threadStart)
		{
			Debug.Log ("Start");
			AbortThread (ref thread);
			thread = new Thread (threadStart);
			thread.Start ();
		}

		private void AbortThread (ref Thread thread)
		{
			Debug.Log ("End");
			if (thread != null)
			{
				thread.Abort ();
				thread = null;
			}
		}

		private void DownloadFile (string url, string savepath)
		{
			try
			{
				Uri uri = new Uri (url);
				HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (uri);
				long countLength = request.GetResponse().ContentLength;


				//check local data's size
				long lStartCount = 0;
				FileStream fileStream;
				if (File.Exists (savepath))
				{
					fileStream = File.OpenWrite (savepath);
					lStartCount = fileStream.Length;
					if (lStartCount >= countLength)
					{
						Debug.Log (savepath + " has Exist....");
						return;
					}
					fileStream.Seek (lStartCount, SeekOrigin.Current);//store current file instream..
				}
				else
				{
					fileStream = new FileStream (savepath, FileMode.Create);
				}

				if (lStartCount > 0)
				{
					request.AddRange ((int) lStartCount);
					Debug.Log (lStartCount);
				}

				//request server
				Stream stream = request.GetResponse ().GetResponseStream ();
				int len = Common.BUFF_SIZE;

				byte[] nbytes = new byte[len];
				int nReadSize = 0;
				nReadSize = stream.Read(nbytes, 0, len);

				string data = "";
				while (nReadSize > 0)
				{
					fileStream.Write(nbytes, 0, nReadSize);
					nReadSize = stream.Read(nbytes, 0, len);
					data = fileStream.Length / Common.M_SIZE + "MB /" + countLength / Common.M_SIZE + "MB";
					Debug.Log (data + "____" + nReadSize);
					Thread.Sleep (15);
				}
				stream.Close();
				fileStream.Close();


				Debug.Log (countLength);
			}
			catch (System.Exception e)
			{
				Debug.LogError (e);
			}
		}
	}
}