using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialStartTrigger : MonoBehaviour
{
	public bool isFinal;

	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnEnterTutorialBlockEvent(((Component)((Component)this).transform.root).gameObject));
			if (isFinal)
			{
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnFinishTutorialEvent());
				MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.Start);
			}
		}
	}
}
