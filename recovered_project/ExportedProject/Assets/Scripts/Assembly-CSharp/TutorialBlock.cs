using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialBlock : MonoBehaviour
{
	public bool givingShield;

	public bool isFinal;

	private void OnEnterBlock()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnEnterTutorialBlockEvent(((Component)this).gameObject));
		if (isFinal)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnFinishTutorialEvent());
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.Start);
		}
		if (givingShield)
		{
			MonoSingleton<UserData>.instance.ShieldCount++;
		}
	}

	private void OnExitBlock()
	{
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
