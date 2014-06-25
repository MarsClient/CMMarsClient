using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

	private Dictionary<string, List<GameObject>> objects = new Dictionary<string, List<GameObject>>();

	private static ObjectPool _Instance;
	public static ObjectPool Instance
	{
		get{
			if (_Instance == null)
			{
				_Instance = new ObjectPool ();
			}
			return _Instance;
		}
	}

	public void Clean ()
	{
		_Instance = null;
	}

	public GameObject LoadObject (GameObject path)
	{
		return LoadObject (path, Vector3.zero);
	}

	public GameObject LoadObject (GameObject path, Vector3 pos)
	{
		List<GameObject> gos = null;
		GameObject go;
		if (objects.TryGetValue (path.name, out gos) == false)
		{
			gos = new List<GameObject>();
		}
		foreach (GameObject g in gos)
		{
			if (g.activeSelf == false)
			{
				g.SetActive (true);
				g.transform.position = pos;
				return g;
			}
		}
		go = path;//Resources.Load (path, typeof(GameObject)) as GameObject;
		go = GameObject.Instantiate (go) as GameObject;//NGUITools.AddChild(go);
		go.transform.position = pos;
		gos.Add (go);
		objects[path.name] = gos;
		return go;
	}

	public void Clear ()
	{
		objects.Clear ();
	}
}
