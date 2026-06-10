using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnEnterTutorialBlockEvent : BaseEvent
{
	private GameObject blockObject;

	public OnEnterTutorialBlockEvent(GameObject boIn)
	{
		blockObject = boIn;
	}

	public override object GetData()
	{
		return blockObject;
	}
}
