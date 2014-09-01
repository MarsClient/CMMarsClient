using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace GmUpdate
{
	public class GameUpdate : MonoBehaviour
	{
		private static GameUpdate mInstance;
		public  static GameUpdate instance
		{
			get
			{
				if (mInstance == null)
					mInstance = new GameObject("GameUpdate").AddComponent<GameUpdate>();
				return mInstance;
			}
		}

		private List<GameUpdateListeners> gameUpdateListeners = new List<GameUpdateListeners>();

#region ResDownload
		public void StartResDownload ()
		{
			StartCoroutine (FPointDownload (Common.URL + Common.ZIP_NAME, Common.STORE_PATH_ZIP, Common.STORE_PATH_ZIP, Common.UN_ZIP_FILE_PATH));
		}

#endregion 


#region Thread Func
//		IEnumerator Run ()
//		{
//			return FPointDownload (Common.URL + Common.ZIP_NAME, Common.STORE_PATH_ZIP);
//		}
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
		void UnZipFile (float progress, string info)
		{
			for (int i = 0; i < gameUpdateListeners.Count; i++)
			{
				GameUpdateListeners listeners = gameUpdateListeners[i];
				listeners.UnZipFile (progress, info);
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

		public IEnumerator FPointDownload(string uri,string saveFile, string inFile, string outFile)
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
					DownloadFileFinish ();
					StartCoroutine (UnZip (inFile, outFile));
					//StartCoroutine (UnZip (Common.STORE_PATH_ZIP, Common.UN_ZIP_FILE_PATH));
					yield break;
				}
				else
				{
					fs.Seek(lStartPos, System.IO.SeekOrigin.Current); //移动文件流中的当前指针 
				}
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
			StartCoroutine (UnZip (inFile, outFile));
		}

		IEnumerator UnZip (string inFile, string outFile)
		{
			if (!Directory.Exists(outFile))
				Directory.CreateDirectory(outFile);
			
			ZipInputStream s = new ZipInputStream(File.OpenRead(inFile));
			
			ZipEntry theEntry;

			string root = "";
			float index = 0;
			float progress = 0;
			string info = "";
			while ((theEntry = s.GetNextEntry()) != null)
			{
				
				string directoryName = Path.GetDirectoryName(theEntry.Name);
				string fileName = Path.GetFileName(theEntry.Name);
				string[] paths = theEntry.Name.Split ('/');
				info = paths[paths.Length - 1].Replace (".assetBundle", "");

				if (directoryName != String.Empty)
					Directory.CreateDirectory(outFile + directoryName);
				
				if (fileName != String.Empty)
				{
					FileStream streamWriter = File.Create(outFile + theEntry.Name);
					
					int size = 2048;
					byte[] data = new byte[2048];
					while (true)
					{
						size = s.Read(data, 0, data.Length);
						if (size > 0)
						{
							streamWriter.Write(data, 0, size);
						}
						else
						{
							break;
						}
						//float progress = (float)streamWriter.Length / (float)theEntry.Size;
						Debug.Log (theEntry.Name + "____"  + streamWriter.Length + "____" + theEntry.Size + "____" + progress + "____" + paths[0] + "____" + paths[1]);
						UnZipFile (progress, info);
					}
					streamWriter.Close();
				}

				yield return false;
			}
			s.Close();
			UnZipFileFinish ();
		}

		void OnApplicationQuit()
		{
			StopCoroutine("FPointDown");
		}
	}
}