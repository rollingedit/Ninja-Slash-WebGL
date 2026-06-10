using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ZombieTrigger : MonoBehaviour
{
	public ZombieType zName;

	public GameObject parentObject;

	public bool isShurikenTarget;

	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterZombie", (object)new ZombieData(parentObject, zName), (SendMessageOptions)1);
	}

	private void OnCrashedByShuriken()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenKilledZombieEvent(zName));
	}

	private void OnEnterShurikenRange()
	{
		if (isShurikenTarget)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenRangeEvent(new ShurikenRangeData(true, parentObject)));
		}
	}

	private void OnExitShurikenRange()
	{
		if (isShurikenTarget)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenRangeEvent(new ShurikenRangeData(false, parentObject)));
		}
	}
}
