using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnSpawnObjectListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		object data = eventIn.GetData();
		((Component)this).SendMessage("OnSpawnObject", (data is GameObject) ? data : null);
		return false;
	}
}
