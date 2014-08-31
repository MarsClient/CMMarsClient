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
		private static GameUpdate mInstance;
		public static GameUpdate instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = new GameObject ("~GameUpdate").AddComponent<GameUpdate>();
				}
				return mInstance;
			}
		}

		private List<GameUpdateListeners> gameUpdateListeners = new List<GameUpdateListeners>();

		/*private Thread apkThread;//for android
		private Thread resThread;// android & IOS
		private Thread unZipThread;// android & IOS*/

		void Start ()
		{
			StartCoroutine (FPointDownload (Common.URL + Common.ZIP_NAME, Common.STORE_PATH));
		}

#region ResDownload
		public void StartResDownload ()
		{
			Run ();
		}

		/*public void AbortResDownload ()
		{
			AbortThread (ref resThread);
		}*/
#endregion 


#region Thread Func
		void Run ()
		{
			//StartCoroutine (FPointDownload (Common.URL + Common.ZIP_NAME, Common.STORE_PATH));
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
			while (nReadSize > 0)
			{
				fs.Write(nbytes, 0, nReadSize);
				nReadSize = ns.Read(nbytes, 0, len);
				string t = "已下载:" + fs.Length / 1024 + "kb /" + countLength / 1024 + "kb";
				DownloadFile ((float) fs.Length / (float) countLength, t);
				yield return false;
			}
			ns.Close();
			fs.Close();
		}

		void OnApplicationQuit()
		{
			print("stop");
			StopCoroutine("FPointDown");
		}
	}
}