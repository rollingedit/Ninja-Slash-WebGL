using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MissionLabel : MonoBehaviour
{
	public UILabel uiLabel;

	private void OnGetObjectFromPool()
	{
		SetLabel();
	}

	private void OnMissionComplete(Mission mission)
	{
		SetLabel();
	}

	private void OnGameOver()
	{
		SetLabel();
	}

	private void SetLabel()
	{
		Mission mission = MonoSingleton<MissionManager>.instance.missions[MonoSingleton<UserData>.instance.MissionProgressId];
		uiLabel.text = mission.desc + " ";
		if (MonoSingleton<MissionManager>.instance.GetCurrentMission().type != Mission.Type.allClear)
		{
			UILabel uILabel = uiLabel;
			string text = uILabel.text;
			uILabel.text = text + "(" + mission.progress + "/" + mission.targetProgress + ")";
		}
	}
}
