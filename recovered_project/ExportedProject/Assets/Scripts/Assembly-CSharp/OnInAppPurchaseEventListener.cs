using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnInAppPurchaseEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnInAppPurchase", (object)(eventIn.GetData() as string));
		return false;
	}
}
