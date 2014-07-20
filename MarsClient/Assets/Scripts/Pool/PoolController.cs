using UnityEngine;
using System.Collections;

public class PoolController : MonoBehaviour {

	private string mPath;
	private GameObject m_go;
	public void Init (string path)
	{
		this.mPath = path;
		this.m_go = gameObject;
	}

	public void Release ()
	{
		PoolManager.Instance.Release (mPath, m_go);
	}

}
