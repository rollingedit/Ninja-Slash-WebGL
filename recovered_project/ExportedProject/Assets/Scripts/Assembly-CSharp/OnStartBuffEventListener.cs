using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnStartBuffEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnStartBuff", eventIn.GetData());
		return false;
	}
}
