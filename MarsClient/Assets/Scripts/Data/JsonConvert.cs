using UnityEngine;
using System.Collections;

using JsonFx.Json;

public class JsonConvert
{
	public static string SerializeObject (object o)
	{
		return JsonWriter.Serialize (o);
	}
	
	public static T DeserializeObject<T> (string json)
	{
		return (T)JsonReader.Deserialize (json, typeof (T));
	}
}
