using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnGameOverEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent evt)
	{
		((Component)this).BroadcastMessage("OnGameOver", (object)(evt.GetData() as GameOverResult), (SendMessageOptions)1);
		return false;
	}
}
