using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShurikenRangeData
{
	public bool isEntered;

	public GameObject gameObject;

	public ShurikenRangeData(bool isEnteredIn, GameObject gameObjectIn)
	{
		isEntered = isEnteredIn;
		gameObject = gameObjectIn;
	}
}
