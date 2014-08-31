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

		/*private Thread apkThread;//for android
		private Thread resThread;// android & IOS
		private Thread unZipThread;// android & IOS*/

#region ResDownload
		public IEnumerator StartResDownload ()
		{
			return Run ();
		}

		/*public void AbortResDownload ()
		{
			AbortThread (ref resThread);
		}*/
#endregion 


#region Thread Func
		IEnumerator Run ()
		{
			return DownloadFile (Common.URL + Common.ZIP_NAME, Common.STORE_PATH);
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


		void DownloadFile (float progress, string info)
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.DownloadFile (progress, info);
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


		/*private void StartThread (ref Thread thread, ThreadStart threadStart)
		{
			AbortThread (ref thread);
			thread = new Thread (threadStart);
			thread.Start ();
		}

		private void AbortThread (ref Thread thread)
		{
			if (thread != null)
			{
				thread.Abort ();
				thread = null;
			}
		}*/

		private IEnumerator DownloadFile (string url, string savepath)
		{
			Uri uri = new Uri (url);
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (uri);
			long countLength = request.GetResponse().ContentLength;

			bool isSuccess = false;
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
					isSuccess = true;
				}
				if (isSuccess == false)
				{
					fileStream.Seek (lStartCount, SeekOrigin.Current);//store current file instream..
				}
			}
			else
			{
				fileStream = new FileStream (savepath, FileMode.Create);
			}
			if (isSuccess == false)
			{
				if (lStartCount > 0)
				{
					request.AddRange ((int) lStartCount);
				}

				//request server
				Stream stream = request.GetResponse ().GetResponseStream ();
				int len = Common.BUFF_SIZE;

				byte[] nbytes = new byte[len];
				int nReadSize = 0;
				nReadSize = stream.Read(nbytes, 0, len);

				string data = "";

				long cur = 0;
				long end = 0;
				float progress = 0;
				while (cur < countLength)
				{
					fileStream.Write(nbytes, 0, nReadSize);
					nReadSize = stream.Read(nbytes, 0, len);

					cur = fileStream.Length;
					end = countLength;
					progress = (float) cur / (float) end;
					data = cur / Common.M_SIZE + "MB /" + end / Common.M_SIZE + "MB";
					DownloadFile (progress, data);
					Debug.Log (data + "____" + nReadSize + "____" + progress);
					yield return new WaitForSeconds (0);
				}
				stream.Close();
				fileStream.Close();
				isSuccess = true;
			}
			DownloadFileFinish ();
		}
	}
}