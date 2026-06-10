using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnShieldEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		switch ((ShieldState)(int)eventIn.GetData())
		{
		case ShieldState.start:
			((Component)this).BroadcastMessage("OnShieldStart", (SendMessageOptions)1);
			break;
		case ShieldState.finish:
			((Component)this).BroadcastMessage("OnShieldFinish", (SendMessageOptions)1);
			break;
		case ShieldState.coolTimeStart:
			((Component)this).BroadcastMessage("OnShieldCoolTimeStart", (SendMessageOptions)1);
			break;
		case ShieldState.coolTimeFinish:
			((Component)this).BroadcastMessage("OnShieldCoolTimeFinish", (SendMessageOptions)1);
			break;
		}
		return false;
	}
}
