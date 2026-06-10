using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnShurikenPooledEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).BroadcastMessage("OnShurikenPooled", (SendMessageOptions)1);
		return false;
	}
}
