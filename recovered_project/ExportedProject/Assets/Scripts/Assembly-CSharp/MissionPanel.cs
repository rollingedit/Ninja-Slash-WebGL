using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MissionPanel : MonoBehaviour
{
	public UILabel missionNumberLabel;

	public UILabel missionContentLabel;

	public UILabel missionProgressLabel;

	public UILabel rewardLabel;

	public GameObject rewardBackground;

	private void OnGetObjectFromPool()
	{
		SetLabel();
	}

	private void OnMissionComplete()
	{
		SetLabel();
	}

	private void SetLabel()
	{
		Mission mission = MonoSingleton<MissionManager>.instance.missions[MonoSingleton<UserData>.instance.MissionProgressId];
		missionNumberLabel.text = string.Concat((object)"Mission ", (object)mission.id);
		missionContentLabel.text = mission.desc;
		if (mission.type != Mission.Type.allClear)
		{
			missionNumberLabel.text = string.Concat((object)"Mission ", (object)mission.id);
			missionProgressLabel.text = "(" + mission.progress + "/" + mission.targetProgress + ")";
			rewardLabel.text = string.Concat((object)"x", (object)(mission.id + 1));
			rewardBackground.SetActive(true);
		}
		else
		{
			missionNumberLabel.text = "Mission";
			missionProgressLabel.text = string.Empty;
			rewardLabel.text = string.Empty;
			rewardBackground.SetActive(false);
		}
	}
}
