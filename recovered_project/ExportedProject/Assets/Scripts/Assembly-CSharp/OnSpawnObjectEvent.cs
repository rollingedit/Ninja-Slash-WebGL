using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnSpawnObjectEvent : BaseEvent
{
	private GameObject spawnObject;

	public OnSpawnObjectEvent(GameObject spawnObjectIn)
	{
		spawnObject = spawnObjectIn;
	}

	public override object GetData()
	{
		return spawnObject;
	}
}
