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

	public GameObject LoadObject (string path, Vector3 pos)
	{
		List<GameObject> gos = null;
		GameObject go;
		if (objects.TryGetValue (path, out gos) == false)
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
		go = Resources.Load (path, typeof(GameObject)) as GameObject;
		go = GameObject.Instantiate (go) as GameObject;//NGUITools.AddChild(go);
		go.transform.position = pos;
		gos.Add (go);
		objects[path] = gos;
		return go;
	}
}
