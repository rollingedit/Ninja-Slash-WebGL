using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnChangeMultiplierEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnChangeMultiplier", eventIn.GetData());
		return false;
	}
}
