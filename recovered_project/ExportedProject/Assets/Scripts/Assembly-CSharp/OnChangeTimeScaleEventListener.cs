using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnChangeTimeScaleEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnChangeTimeScale", (object)(eventIn.GetData() as ChangeTimeScaleData));
		return false;
	}
}
