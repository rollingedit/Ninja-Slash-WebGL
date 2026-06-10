using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnResumeEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).BroadcastMessage("OnResume");
		return false;
	}
}
