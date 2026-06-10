using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MissionWindow : MessageWindow
{
	public UILabel missionLabel;

	public UILabel titleLabel;

	public GameObject reward;

	public UILabel rewardLabel;

	private void OnMissionComplete(Mission missionIn)
	{
		titleLabel.text = "Mission Complete";
		missionLabel.text = missionIn.desc;
		rewardLabel.text = string.Concat((object)"x", (object)(missionIn.id + 1));
		reward.SetActive(true);
	}

	private void OnNextMissionAnnounce(Mission missionIn)
	{
		titleLabel.text = "Next Mission";
		missionLabel.text = missionIn.desc;
		reward.SetActive(false);
	}
}
