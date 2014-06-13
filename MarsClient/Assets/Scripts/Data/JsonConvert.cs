using UnityEngine;
using System.Collections;

using Pathfinding.Serialization.JsonFx;

public class JsonConvert
{
	public static string SerializeObject (object o)
	{
		return JsonWriter.Serialize (o);
	}
	
	public static T DeserializeObject<T> (string json)
	{
		return JsonReader.Deserialize<T> (json);
	}
}
