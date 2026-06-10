using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class Ninja
{
	public enum HowToUnlock
	{
		token = 0,
		coin = 1
	}

	[SerializeField]
	public string ninjaName;

	[SerializeField]
	public GameObject ninjaModel;

	[SerializeField]
	public GameObject ninjaRagdoll;

	[SerializeField]
	public HowToUnlock howToUnlock;

	[SerializeField]
	public int requiredToken;

	[SerializeField]
	public int unlockCost;

	[HideInInspector]
	public int curToken
	{
		get
		{
			return MonoSingleton<UserData>.instance.GetMatchingNinjaToken(ninjaName);
		}
		set
		{
			MonoSingleton<UserData>.instance.SetMatchingNinjaToken(ninjaName, value);
			if (value >= requiredToken)
			{
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnNinjaUnlockedEvent(this));
			}
		}
	}

	[HideInInspector]
	public bool isUnlocked
	{
		get
		{
			return MonoSingleton<UserData>.instance.GetMatchingNinjaIsUnlocked(ninjaName);
		}
		set
		{
			MonoSingleton<UserData>.instance.SetMatchingNinjaIsUnlocked(ninjaName, value);
			if (value)
			{
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnNinjaUnlockedEvent(this));
			}
		}
	}

	public bool IsNinjaUnlocked()
	{
		// PORT TWEAK (user request): all characters unlocked by default in the WebGL port.
		// Original token/coin unlock logic kept below for reference.
		return true;
#pragma warning disable CS0162
		if (howToUnlock == HowToUnlock.token)
		{
			return curToken >= requiredToken;
		}
		if (howToUnlock == HowToUnlock.coin)
		{
			return isUnlocked;
		}
		Debug.LogError((object)"No Matching HowToUnlock!!");
		return false;
#pragma warning restore CS0162
	}
}
