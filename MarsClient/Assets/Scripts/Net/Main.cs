using UnityEngine;
using System.Collections;

public class Main 
{
	private static Main m_instance;
	public static Main Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new Main ();
			}
			return m_instance;
		}
	}

	public Account account;
}
