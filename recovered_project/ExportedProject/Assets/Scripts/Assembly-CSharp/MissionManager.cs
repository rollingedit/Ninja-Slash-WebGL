using System.Collections.Generic;

public class MissionManager : MonoSingleton<MissionManager>
{
	public List<Mission> missions;

	public override void Init()
	{
		Mission currentMission = GetCurrentMission();
		if (currentMission != null)
		{
			if (currentMission.resetOnStart)
			{
				currentMission.ResetProgress();
			}
			else
			{
				currentMission.SetProgress(MonoSingleton<UserData>.instance.MissionProgress);
			}
		}
	}

	public void CompleteCurrentMission()
	{
		Mission currentMission = GetCurrentMission();
		if (currentMission != null)
		{
			currentMission.Complete();
		}
	}

	private void OnGameStart()
	{
		Init();
	}

	public void ProcessMission(Mission.Type typeIn)
	{
		Mission currentMission = GetCurrentMission();
		if (currentMission != null && currentMission.type == typeIn)
		{
			currentMission.IncreamentProgress();
		}
	}

	public void ProcessMission(Mission.Type typeIn, int addValueIn)
	{
		Mission currentMission = GetCurrentMission();
		if (currentMission != null && currentMission.type == typeIn)
		{
			currentMission.AddProgress(addValueIn);
		}
	}

	public void ResetMission(Mission.Type typeIn)
	{
		Mission currentMission = GetCurrentMission();
		if (currentMission != null && currentMission.type == typeIn)
		{
			currentMission.ResetProgress();
		}
	}

	public void OnMissionComplete(Mission missionIn)
	{
		MonoSingleton<UserData>.instance.MissionProgressId++;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnMissionCompleteEvent(missionIn));
	}

	public Mission GetCurrentMission()
	{
		return missions[MonoSingleton<UserData>.instance.MissionProgressId];
	}
}
