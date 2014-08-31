using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;
using GmUpdate;

public class Load : MonoBehaviour {
	
	AsyncOperation l;
	void Start () {
		// Application.backgroundLoadingPriority = ThreadPriority.Low;
		// l =  Application.LoadLevelAsync("game");
		/*StartCoroutine(FPointDown("http://127.0.0.1:8888/index.rar", "d:/a.rar"));
		StartCoroutine(FPointDown(@"http://dl_dir.qq.com/qqfile/qq/QQ2012/QQ2012Beta3.exe", "d:/qq.rar"));*/
		
	}
	
	
	void OnGUI () {
		if (GUILayout.Button(t))
		{
			StartCoroutine(FPointDown (Common.URL + Common.ZIP_NAME, Common.STORE_PATH) );
		}
	}
	void OnApplicationQuit()
	{
		print("stop");
		StopCoroutine("FPointDown");
	}
	string t = "";
	IEnumerator  downfile(string url, string LocalPath)
	{
		
		Uri u = new Uri(url);
		HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);
		mRequest.Method = "GET";
		mRequest.ContentType = "application/x-www-form-urlencoded";
		
		HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();
		
		Stream sIn = wr.GetResponseStream();
		FileStream fs = new FileStream(LocalPath, FileMode.Create, FileAccess.Write);
		
		long length = wr.ContentLength;
		long i = 0;
		decimal j = 0;
		
		while (i < length)
		{
			byte[] buffer = new byte[1024];
			i += sIn.Read(buffer, 0, buffer.Length);
			fs.Write(buffer, 0, buffer.Length);
			
			if ((i % 1024) == 0)
			{
				j = Math.Round(Convert.ToDecimal((Convert.ToDouble(i) / Convert.ToDouble(length)) * 100), 4);
				t= "当前下载文件大小:" + length.ToString() + "字节   当前下载大小:" + i + "字节 下载进度" + j.ToString() + "%";
				
			}
			else
			{
				t = "当前下载文件大小:" + length.ToString() + "字节   当前下载大小:" + i + "字节";
			}
			yield return false;
			
			
		}
		
		sIn.Close();
		wr.Close();
		fs.Close();
		
	}
	
	//断点下载
	IEnumerator FPointDown(string uri,string saveFile)
	{
		//打开网络连接 
		System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
		System.Net.HttpWebRequest requestGetCount = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
		long countLength = requestGetCount.GetResponse().ContentLength;
		
		//打开上次下载的文件或新建文件 
		long lStartPos = 0;
		System.IO.FileStream fs;
		if (System.IO.File.Exists(saveFile))
		{
			fs = System.IO.File.OpenWrite(saveFile);
			lStartPos = fs.Length;
			if (countLength - lStartPos <= 0)
			{
				fs.Close();
				t = "已经";
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
			request.AddRange((int)lStartPos); //设置Range值
			print(lStartPos);
		}
		
		//向服务器请求，获得服务器回应数据流 
		System.IO.Stream ns = request.GetResponse().GetResponseStream();
		int len = 1024 * 8;
		
		byte[] nbytes = new byte[len];
		int nReadSize = 0;
		nReadSize = ns.Read(nbytes, 0, len);
		while (nReadSize > 0)
		{
			fs.Write(nbytes, 0, nReadSize);
			nReadSize = ns.Read(nbytes, 0, len);
			t = "已下载:" + fs.Length / 1024 + "kb /" + countLength / 1024 + "kb";
			yield return false;
		}
		ns.Close();
		fs.Close();
	}
}
