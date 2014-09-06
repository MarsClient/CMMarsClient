using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pool manager.
/// this class is One only in fight scene
/// when load new scene,this class will release.
/// </summary>

public class PoolManager : MonoBehaviour {

	public static PoolManager Instance;
	void Awake () { Instance = this; }
	void OnDestroy() { Instance = null; }

	private Dictionary<string, LinkedList<GameObject>> pools = new Dictionary<string, LinkedList<GameObject>>();

	public delegate void LoadingGameObjectDone (GameObject target);

	public void LoadGameObject (string path, LoadingGameObjectDone loadObjDelegate)
	{
		LoadGameObject (path, loadObjDelegate, true, null);
	}
	public void LoadGameObject (string path, LoadingGameObjectDone loadObjDelegate, string Constant)
	{
		LoadGameObject (path, loadObjDelegate, false, Constant);
	}
	private void LoadGameObject (string path, LoadingGameObjectDone loadObjDelegate, bool isResource, string Constant)
	{
		GameObject m_go = null;
		LinkedList<GameObject> goList = null;

		if (pools.TryGetValue (path, out goList) == false)
		{
			goList = new LinkedList<GameObject> ();
			pools.Add (path, goList);
		}

		if (goList.Count > 0)
		{
			m_go = goList.First.Value;
			goList.RemoveFirst ();
			pools[path] = goList;
		}
		else
		{
			GameObject res_go;

			if (isResource == false)
			{
				AssetLoader.Instance.DownloadAssetbundle (new string[]{Constant + path}
				, (List<object> objs)=>
				{
					res_go = (GameObject) objs[0];
					m_go = ObjInstantiate (path, res_go);
					LoadAssetBundle (m_go, loadObjDelegate);
				});
				return;
			}
			else
			{
				res_go = Resources.Load (path) as GameObject;
				m_go = ObjInstantiate (path, res_go);
			}
		}
		LoadAssetBundle (m_go, loadObjDelegate);

	}

	private void LoadAssetBundle (GameObject m_go, LoadingGameObjectDone loadObjDelegate)
	{
		m_go.SetActive (true);
		if (loadObjDelegate != null)
		{
			loadObjDelegate (m_go);
		}
	}

	private GameObject ObjInstantiate (string path, GameObject res_go)
	{
		GameObject m_go = GameObject.Instantiate(res_go) as GameObject;
		PoolController pc = m_go.AddComponent<PoolController> ();
		pc.Init (path);
		return m_go;
	}

	public void Release (string path, GameObject go)
	{
		LinkedList<GameObject> goList = null;
		
		if (pools.TryGetValue (path, out goList) == false)
		{
			goList = new LinkedList<GameObject>();
		}
		if (go.transform.parent != null) { go.transform.parent = null; }
		goList.AddLast (go);
		go.SetActive (false);
		pools[path] = goList;
	}

	public void Clear ()
	{
		pools.Clear ();
	}
}
