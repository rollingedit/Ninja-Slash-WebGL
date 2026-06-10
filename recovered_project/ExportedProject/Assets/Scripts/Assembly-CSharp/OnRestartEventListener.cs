using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnRestartEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent evt)
	{
		((Component)this).BroadcastMessage("OnRestart");
		return false;
	}
}
