using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterScoreListener : MonoBehaviour
{
	public int zomblingScore;

	public int lankieScore;

	public int goliathScore;

	public int fatsoScore;

	public int sweeperScore;

	public int berserkerScore;

	public int coinScore;

	public float comboLimitTime;

	public float comboLimitTimeOnWindBuff;

	public int multiplierBoostQuantity;

	private bool isEnabled;

	private int curCombo;

	private float comboRemainingTime;

	private int curMultiplier;

	private bool isFlameBuffOn;

	private bool isGoldBuffOn;

	private bool isWindBuffOn;

	private bool isMultiplierBoosterOn;

	private int multiplierLowerLimit;

	private int multiplierUpperLimit;

	public CharacterRecord characterRecord;

	private void OnGameStart()
	{
		isEnabled = true;
		SetMultiplierLimit();
	}

	private void OnUseMultiplierBooster()
	{
		isMultiplierBoosterOn = true;
		SetMultiplierLimit();
		curMultiplier = multiplierLowerLimit;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangeMultiplierEvent(new MultiplierData(multiplierLowerLimit, multiplierUpperLimit * ((!isFlameBuffOn) ? 1 : 2), curMultiplier)));
	}

	private void SetMultiplierLimit()
	{
		multiplierLowerLimit = (isMultiplierBoosterOn ? multiplierBoostQuantity : 0);
		multiplierUpperLimit = MonoSingleton<MissionManager>.instance.GetCurrentMission().id + (isMultiplierBoosterOn ? multiplierBoostQuantity : 0);
	}

	private void OnMissionComplete()
	{
		if (isEnabled)
		{
			SetMultiplierLimit();
		}
	}

	private void OnGetScore(int scoreIn)
	{
		int num = ((!isFlameBuffOn) ? 1 : 2);
		curMultiplier = Mathf.Clamp(curMultiplier + num, multiplierLowerLimit, multiplierUpperLimit * ((!isFlameBuffOn) ? 1 : 2));
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangeMultiplierEvent(new MultiplierData(multiplierLowerLimit, multiplierUpperLimit * ((!isFlameBuffOn) ? 1 : 2), curMultiplier)));
		characterRecord.AddScore(scoreIn * curMultiplier);
		curCombo += num;
		comboRemainingTime = Mathf.Max(comboRemainingTime, (!isWindBuffOn) ? comboLimitTime : comboLimitTimeOnWindBuff);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnComboEvent(curCombo));
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.combo, num);
	}

	private void Update()
	{
		if (isEnabled)
		{
			if (comboRemainingTime > 0f)
			{
				comboRemainingTime -= Time.deltaTime;
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnUpdateTimeEvent(new UpdateTimeData(comboRemainingTime, (!isWindBuffOn) ? comboLimitTime : comboLimitTimeOnWindBuff, UpdateTimeTarget.combo)));
			}
			else if (curCombo > 0)
			{
				OnEndCombo();
			}
		}
	}

	private void OnEndCombo()
	{
		characterRecord.AddCombo(curCombo);
		curCombo = 0;
		curMultiplier = multiplierLowerLimit;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnComboEvent(curCombo));
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangeMultiplierEvent(new MultiplierData(multiplierLowerLimit, multiplierUpperLimit * ((!isFlameBuffOn) ? 1 : 2), curMultiplier)));
		MonoSingleton<MissionManager>.instance.ResetMission(Mission.Type.combo);
	}

	private void OnKilledZombie(ZombieType zName)
	{
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashAnyZombie);
		characterRecord.IncreamentKill();
		if (((global::System.Enum)zName).Equals((object)ZombieType.zombling))
		{
			OnGetScore(zomblingScore);
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashZombling);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.goliath))
		{
			OnGetScore(goliathScore);
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashGoliath);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.lankie))
		{
			OnGetScore(lankieScore);
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashLanky);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.fatso))
		{
			OnGetScore(fatsoScore);
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashFatso);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.sweeper_head) || ((global::System.Enum)zName).Equals((object)ZombieType.sweeper_body))
		{
			OnGetScore(sweeperScore);
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.slashSweeper);
		}
		else if (((global::System.Enum)zName).Equals((object)ZombieType.berserker))
		{
			OnGetScore(berserkerScore);
		}
	}

	private void OnEnterBamboo()
	{
		OnGetScore(0);
	}

	private void OnEnterGateDoor(GameObject door)
	{
		OnGetScore(0);
	}

	private void OnGetCoin()
	{
		characterRecord.AddScore(coinScore * curMultiplier * ((!isGoldBuffOn) ? 1 : 2));
	}

	private void OnShurikenKilledZombie(ZombieType zName)
	{
		OnKilledZombie(zName);
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Flame))
		{
			isFlameBuffOn = true;
		}
		else if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Wind))
		{
			isWindBuffOn = true;
		}
		else if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = true;
		}
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Flame))
		{
			isFlameBuffOn = false;
		}
		else if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Wind))
		{
			isWindBuffOn = false;
		}
		else if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = false;
		}
	}

	private void OnDead()
	{
		OnEndCombo();
		isEnabled = false;
	}

	private void OnRestart()
	{
		isEnabled = true;
	}

	private void OnPutObjectIntoPool()
	{
		curCombo = 0;
		comboRemainingTime = 0f;
		curMultiplier = 0;
		isEnabled = false;
		isMultiplierBoosterOn = false;
	}
}
