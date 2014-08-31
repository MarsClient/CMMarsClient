using UnityEngine;
using System.Collections;

namespace GmUpdate
{
	public interface GameUpdateListeners
	{
		void DownloadFile (float progress);
		void DownloadFileFinish ();
		void UnZipFile ();
		void UnZipFileFinish ();
	}
}
