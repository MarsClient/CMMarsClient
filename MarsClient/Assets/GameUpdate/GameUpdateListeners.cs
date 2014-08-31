﻿using UnityEngine;
using System.Collections;

namespace GmUpdate
{
	public interface GameUpdateListeners
	{
		void DownloadFile (float progress, string info);
		void DownloadFileFinish ();
		void UnZipFile ();
		void UnZipFileFinish ();
	}
}
