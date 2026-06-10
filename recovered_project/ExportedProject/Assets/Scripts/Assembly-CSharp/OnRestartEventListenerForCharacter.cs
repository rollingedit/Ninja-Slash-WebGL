using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnRestartEventListenerForCharacter : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).BroadcastMessage("OnGameStart");
		return false;
	}
}
