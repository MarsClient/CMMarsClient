using UnityEngine;
using System.Collections;

namespace GmUpdate
{
	/*ftp://qq459127484:kanni789@002.3vftp.com /MarsRes.zip*/
	/*http://edge.cdn.twgate.net/hh/hh/low/GodSlayerResource85.zip*/
	public class Common
	{
		public const string URL = "http://edge.cdn.twgate.net/hh/hh/low";
		public const string ZIP_NAME = "/GodSlayerResource85.zip";
		public const int BUFF_SIZE = 1024 * 8;
		public const int M_SIZE = 1024;
		
		public static string STORE_PATH_ZIP
		{
			get
			{
				string storePath = "D:/Unity_Space/res" + ZIP_NAME;
				return storePath;
			}
		}

		public static string UN_ZIP_FILE_PATH
		{
			get
			{
				string storePath = "D:/Unity_Space/res/MarsClientRes/";
				return storePath;
			}
		}
	}
}
