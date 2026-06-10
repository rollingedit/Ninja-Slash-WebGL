using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShurikenTarget : MonoBehaviour
{
	private void OnEnterShurikenRange()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenRangeEvent(new ShurikenRangeData(true, ((Component)this).gameObject)));
	}

	private void OnExitShurikenRange()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenRangeEvent(new ShurikenRangeData(false, ((Component)this).gameObject)));
	}
}
