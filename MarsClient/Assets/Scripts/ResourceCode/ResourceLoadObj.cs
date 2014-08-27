using UnityEngine;
using System.Collections;

public class ResourceLoadObj
{
	public static T SetResourceObjInstance<T> (string path, T t) where T : Component
	{
		if (t == null)
		{
			GameObject resGo = Resources.Load (path) as GameObject;
			t = resGo.GetComponent<T>();
		}
		return t;
	}
}
