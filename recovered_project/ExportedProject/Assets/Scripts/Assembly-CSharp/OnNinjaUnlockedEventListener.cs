using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnNinjaUnlockedEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnNinjaUnlocked", eventIn.GetData());
		return false;
	}
}
