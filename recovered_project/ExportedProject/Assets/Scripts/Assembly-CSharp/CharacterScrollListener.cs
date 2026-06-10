using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterScrollListener : MonoBehaviour
{
	private void OnGetScroll(ScrollElement elementIn)
	{
		switch (elementIn)
		{
		case ScrollElement.Flame:
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getFlameScroll);
			break;
		case ScrollElement.Gold:
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getGoldScroll);
			break;
		case ScrollElement.Thunder:
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getThunderScroll);
			break;
		case ScrollElement.Wind:
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getWindScroll);
			break;
		}
		((Component)this).BroadcastMessage("OnStartBuff", (object)elementIn);
	}
}
