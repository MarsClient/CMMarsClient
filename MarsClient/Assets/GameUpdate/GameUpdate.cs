using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GmUpdate
{
	public class GameUpdate : MonoBehaviour
	{
		public readonly static GameUpdate instance = new GameUpdate();
		private List<GameUpdateListeners> gameUpdateListeners = new List<GameUpdateListeners>();

#region ResDownload
		public IEnumerator StartResDownload ()
		{
			return Run ();
		}

#endregion 


#region Thread Func
		IEnumerator Run ()
		{
			return FPointDownload (Common.URL + Common.ZIP_NAME, Common.STORE_PATH);
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

		public IEnumerator FPointDownload(string uri,string saveFile)
		{
			System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
			System.Net.HttpWebRequest requestGetCount = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
			long countLength = requestGetCount.GetResponse().ContentLength;
			

			long lStartPos = 0;
			System.IO.FileStream fs;
			if (System.IO.File.Exists(saveFile))
			{
				fs = System.IO.File.OpenWrite(saveFile);
				lStartPos = fs.Length;
				if (countLength - lStartPos <= 0)
				{
					DownloadFileFinish ();
					fs.Close();
					yield break;
				}
				fs.Seek(lStartPos, System.IO.SeekOrigin.Current); //移动文件流中的当前指针 
			}
			else
			{
				fs = new System.IO.FileStream(saveFile, System.IO.FileMode.Create);
			}
			
			
			if (lStartPos > 0)
			{
				request.AddRange((int)lStartPos); 
				Debug.Log(lStartPos);
			}
			

			System.IO.Stream ns = request.GetResponse().GetResponseStream();
			int len = Common.BUFF_SIZE;
			
			byte[] nbytes = new byte[len];
			int nReadSize = 0;
			nReadSize = ns.Read(nbytes, 0, len);

			string temp = Localization.Get("gameUpdate.download.progress");
			while (nReadSize > 0)
			{
				fs.Write(nbytes, 0, nReadSize);
				nReadSize = ns.Read(nbytes, 0, len);
				string t =string.Format (temp, fs.Length / 1024 + "kb /" + countLength / 1024 + "kb");
				DownloadFile ((float) fs.Length / (float) countLength, t);
				yield return false;
			}
			ns.Close();
			fs.Close();

			DownloadFileFinish ();
		}

		void OnApplicationQuit()
		{
			StopCoroutine("FPointDown");
		}
	}
}