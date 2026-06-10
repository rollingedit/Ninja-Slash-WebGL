using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterItemUseListener : MonoBehaviour
{
	private bool isShieldOn;

	private bool isShieldEnabledOnTutorial;

	private void OnUseMultiplierBooster()
	{
		MonoSingleton<UserData>.instance.MultiplierBoosterCount--;
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.useMultiplierBooster);
	}

	private void OnDoShield()
	{
		if (isShieldOn)
		{
			return;
		}
		if (!MonoSingleton<UserData>.instance.IsTutorialDone)
		{
			if (isShieldEnabledOnTutorial)
			{
				isShieldOn = true;
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.start));
			}
		}
		else if (MonoSingleton<UserData>.instance.ShieldCount > 0)
		{
			isShieldOn = true;
			MonoSingleton<UserData>.instance.ShieldCount--;
			MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.useShadowCloak);
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.start));
		}
	}

	private void OnShieldCoolTimeFinish()
	{
		isShieldOn = false;
	}

	private void OnGameStart()
	{
		isShieldOn = false;
		isShieldEnabledOnTutorial = false;
	}

	private void OnEnterShieldTutorialBlock()
	{
		isShieldEnabledOnTutorial = true;
	}
}
