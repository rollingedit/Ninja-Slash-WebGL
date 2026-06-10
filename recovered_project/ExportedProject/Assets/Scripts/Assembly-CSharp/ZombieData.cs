using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ZombieData
{
	public GameObject zombieObject;

	public ZombieType zombieType;

	public ZombieData(GameObject go, ZombieType type)
	{
		zombieObject = go;
		zombieType = type;
	}
}
