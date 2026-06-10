using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OnStartDownloadingEventListener : BaseEventListener
{
	public override bool HandleEvent(IEvent eventIn)
	{
		((Component)this).SendMessage("OnStartDownloading");
		return false;
	}
}
