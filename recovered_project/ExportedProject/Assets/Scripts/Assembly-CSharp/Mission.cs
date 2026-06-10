using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class Mission
{
	public enum Type
	{
		Start = 0,
		slashZombling = 1,
		slashFatso = 2,
		slashLanky = 3,
		slashGoliath = 4,
		slashSweeper = 5,
		slashAnyZombie = 6,
		getFlameScroll = 7,
		getGoldScroll = 8,
		getWindScroll = 9,
		getThunderScroll = 10,
		getLuckyBox = 11,
		getCoin = 12,
		useShadowCloak = 13,
		useMultiplierBooster = 14,
		crashGate = 15,
		slashProp = 16,
		score = 17,
		combo = 18,
		allClear = 19
	}

	[SerializeField]
	public int id;

	[SerializeField]
	public string desc;

	[SerializeField]
	public float progress;

	[SerializeField]
	public float targetProgress;

	[SerializeField]
	public Type type;

	[SerializeField]
	public bool resetOnStart;

	public void SetProgress(float progressIn)
	{
		progress = progressIn;
		MonoSingleton<UserData>.instance.MissionProgress = progress;
		if (progress >= targetProgress)
		{
			progress = targetProgress;
			MonoSingleton<UserData>.instance.MissionProgress = 0f;
			MonoSingleton<MissionManager>.instance.OnMissionComplete(this);
		}
	}

	public void Complete()
	{
		SetProgress(targetProgress);
	}

	public void AddProgress(float addValueIn)
	{
		SetProgress(progress + addValueIn);
	}

	public void IncreamentProgress()
	{
		AddProgress(1f);
	}

	public void ResetProgress()
	{
		SetProgress(0f);
	}
}
