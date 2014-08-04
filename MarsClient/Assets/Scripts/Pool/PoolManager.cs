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

	public GameObject LoadGameObject (string path)
	{
		return LoadGameObject (path, null, null);
	}

	public GameObject LoadGameObject (string path, Transform target)
	{
		return LoadGameObject (path, target, null);
	}

	public GameObject LoadGameObject (string path, Transform target, Transform parent)
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
			GameObject res_go = Resources.Load (path) as GameObject;

			/*AssetLoader*/
			AssetLoader.Instance.DownloadAssetbundle (new string[]{"EF/" + path}
			, (List<object> objs)=>
			{
				res_go = (GameObject) objs[0];
				Debug.Log (res_go.name);
			});
			/*AssetLoader*/

			if (res_go == null)
			{
				return null;
			}
			m_go = GameObject.Instantiate(res_go) as GameObject;
			PoolController pc = m_go.AddComponent<PoolController> ();
			pc.Init (path);
		}
		if (target != null)
		{
			m_go.transform.position = target.position;
			m_go.transform.rotation = target.rotation;
		}
		if (parent != null)
		{
			m_go.transform.parent = parent;
		}
		m_go.SetActive (true);
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
}
