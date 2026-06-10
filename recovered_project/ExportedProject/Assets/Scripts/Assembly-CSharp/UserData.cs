using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class UserData : MonoSingleton<UserData>
{
	public class UserDataElement
	{
		private string key;

		private object defaultValue;

		private object cacheValue;

		private bool isDirty;

		public UserDataElement(string keyIn)
		{
			key = keyIn;
			defaultValue = null;
			MonoSingleton<UserData>.instance.allUserData.Add(this);
		}

		public UserDataElement(string keyIn, object dataIn)
		{
			key = keyIn;
			defaultValue = dataIn;
			MonoSingleton<UserData>.instance.allUserData.Add(this);
		}

		public object GetData(global::System.Type typeIn)
		{
			if (cacheValue != null)
			{
				return cacheValue;
			}
			isDirty = false;
			if (typeIn == typeof(int))
			{
				cacheValue = GetInt(key, (defaultValue != null) ? ((int)defaultValue) : 0);
			}
			else if (typeIn == typeof(float))
			{
				cacheValue = GetFloat(key, (defaultValue != null) ? ((float)defaultValue) : 0f);
			}
			else
			{
				if (typeIn != typeof(bool))
				{
					Debug.LogError((object)"Undefined type");
					return null;
				}
				cacheValue = GetBool(key, defaultValue != null && (bool)defaultValue);
			}
			return cacheValue;
		}

		public void Flush()
		{
			if (isDirty)
			{
				global::System.Type type = cacheValue.GetType();
				if (type == typeof(int))
				{
					SetInt(key, (int)cacheValue);
				}
				else if (type == typeof(float))
				{
					SetFloat(key, (float)cacheValue);
				}
				else if (type == typeof(bool))
				{
					SetBool(key, (bool)cacheValue);
				}
				isDirty = false;
			}
		}

		public void SetData(object dataIn)
		{
			cacheValue = dataIn;
			isDirty = true;
		}

		public bool IsKeyMatching(string keyIn)
		{
			return key == keyIn;
		}

		private int GetInt(string keyIn, int defaultValueIn)
		{
			return PlayerPrefs.GetInt(keyIn, defaultValueIn);
		}

		private void SetInt(string keyIn, int dataIn)
		{
			PlayerPrefs.SetInt(keyIn, dataIn);
		}

		private float GetFloat(string keyIn, float defaultValueIn)
		{
			return PlayerPrefs.GetFloat(keyIn, defaultValueIn);
		}

		private void SetFloat(string keyIn, float dataIn)
		{
			PlayerPrefs.SetFloat(keyIn, dataIn);
		}

		private bool GetBool(string keyIn, bool defaultValueIn)
		{
			if (PlayerPrefs.HasKey(keyIn))
			{
				return PlayerPrefs.GetInt(keyIn) == 1;
			}
			return defaultValueIn;
		}

		private void SetBool(string keyIn, bool dataIn)
		{
			PlayerPrefs.SetInt(keyIn, dataIn ? 1 : 0);
		}
	}

	public List<UserDataElement> allUserData = new List<UserDataElement>();

	private List<UserDataElement> ninjaList = new List<UserDataElement>();

	private UserDataElement playedGameCount;

	private UserDataElement missionProgressId;

	private UserDataElement missionProgress;

	private UserDataElement coin;

	private UserDataElement highScore;

	private UserDataElement highCombo;

	private UserDataElement kill;

	private UserDataElement multiplierBoosterCount;

	private UserDataElement shieldCount;

	private UserDataElement flameUpgradeLevel;

	private UserDataElement goldUpgradeLevel;

	private UserDataElement thunderUpgradeLevel;

	private UserDataElement windUpgradeLevel;

	private UserDataElement isBGMOn;

	private UserDataElement isEffectSoundOn;

	private UserDataElement isTutorialDone;

	private UserDataElement selectedNinjaIndex;

	private UserDataElement facebookConnect;

	public bool FacebookConnect
	{
		get
		{
			return (bool)facebookConnect.GetData(typeof(bool));
		}
		set
		{
			facebookConnect.SetData(value);
		}
	}

	public int PlayedGameCount
	{
		get
		{
			return (int)playedGameCount.GetData(typeof(int));
		}
		set
		{
			playedGameCount.SetData(value);
		}
	}

	public int MissionProgressId
	{
		get
		{
			return (int)missionProgressId.GetData(typeof(int));
		}
		set
		{
			missionProgressId.SetData(value);
		}
	}

	public float MissionProgress
	{
		get
		{
			return (float)missionProgress.GetData(typeof(float));
		}
		set
		{
			missionProgress.SetData(value);
		}
	}

	public int Coin
	{
		get
		{
			return (int)coin.GetData(typeof(int));
		}
		set
		{
			coin.SetData(value);
		}
	}

	public int Kill
	{
		get
		{
			return (int)kill.GetData(typeof(int));
		}
		set
		{
			kill.SetData(value);
		}
	}

	public int HighScore
	{
		get
		{
			return (int)highScore.GetData(typeof(int));
		}
		set
		{
			highScore.SetData(value);
		}
	}

	public int HighCombo
	{
		get
		{
			return (int)highCombo.GetData(typeof(int));
		}
		set
		{
			highCombo.SetData(value);
		}
	}

	public int MultiplierBoosterCount
	{
		get
		{
			return (int)multiplierBoosterCount.GetData(typeof(int));
		}
		set
		{
			multiplierBoosterCount.SetData(value);
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnItemNumberChangedEvent());
		}
	}

	public int ShieldCount
	{
		get
		{
			return (int)shieldCount.GetData(typeof(int));
		}
		set
		{
			shieldCount.SetData(value);
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnItemNumberChangedEvent());
		}
	}

	public int FlameUpgradeLevel
	{
		get
		{
			return (int)flameUpgradeLevel.GetData(typeof(int));
		}
		set
		{
			flameUpgradeLevel.SetData(value);
		}
	}

	public int GoldUpgradeLevel
	{
		get
		{
			return (int)goldUpgradeLevel.GetData(typeof(int));
		}
		set
		{
			goldUpgradeLevel.SetData(value);
		}
	}

	public int ThunderUpgradeLevel
	{
		get
		{
			return (int)thunderUpgradeLevel.GetData(typeof(int));
		}
		set
		{
			thunderUpgradeLevel.SetData(value);
		}
	}

	public int WindUpgradeLevel
	{
		get
		{
			return (int)windUpgradeLevel.GetData(typeof(int));
		}
		set
		{
			windUpgradeLevel.SetData(value);
		}
	}

	public bool IsBGMOn
	{
		get
		{
			return (bool)isBGMOn.GetData(typeof(bool));
		}
		set
		{
			isBGMOn.SetData(value);
		}
	}

	public bool IsEffectSoundOn
	{
		get
		{
			return (bool)isEffectSoundOn.GetData(typeof(bool));
		}
		set
		{
			isEffectSoundOn.SetData(value);
		}
	}

	public bool IsTutorialDone
	{
		get
		{
			return (bool)isTutorialDone.GetData(typeof(bool));
		}
		set
		{
			isTutorialDone.SetData(value);
		}
	}

	public int SelectedNinjaIndex
	{
		get
		{
			return (int)selectedNinjaIndex.GetData(typeof(int));
		}
		set
		{
			selectedNinjaIndex.SetData(value);
		}
	}

	private void Awake()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = MonoSingleton<NinjaInfo>.instance.ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Ninja current = enumerator.Current;
				if (current.howToUnlock == Ninja.HowToUnlock.token)
				{
					ninjaList.Add(new UserDataElement(current.ninjaName, 0));
				}
				else if (current.howToUnlock == Ninja.HowToUnlock.coin)
				{
					ninjaList.Add(new UserDataElement(current.ninjaName, false));
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		playedGameCount = new UserDataElement("played_game_count");
		missionProgressId = new UserDataElement("mission_progress_id");
		missionProgress = new UserDataElement("mission_progress");
		coin = new UserDataElement("coin");
		highScore = new UserDataElement("high_score");
		highCombo = new UserDataElement("high_combo");
		multiplierBoosterCount = new UserDataElement("multiplier_booster_count");
		shieldCount = new UserDataElement("shield_count");
		flameUpgradeLevel = new UserDataElement("flame_upgrade_level");
		goldUpgradeLevel = new UserDataElement("gold_upgrade_level");
		thunderUpgradeLevel = new UserDataElement("thunder_upgrade_level");
		windUpgradeLevel = new UserDataElement("wind_upgrade_level");
		isBGMOn = new UserDataElement("is_bgm_on", true);
		isEffectSoundOn = new UserDataElement("is_effect_sound_on", true);
		isTutorialDone = new UserDataElement("is_tutorial_done", false);
		selectedNinjaIndex = new UserDataElement("selected_ninja_index");
		facebookConnect = new UserDataElement("facebook_connect");
		kill = new UserDataElement("kill");
	}

	public int GetMatchingNinjaToken(string ninjaName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UserDataElement current = enumerator.Current;
				if (current.IsKeyMatching(ninjaName))
				{
					return (int)current.GetData(typeof(int));
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Debug.LogError((object)"No Matching Ninja!!");
		return -1;
	}

	public void SetMatchingNinjaToken(string ninjaName, int tokenIn)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UserDataElement current = enumerator.Current;
				if (current.IsKeyMatching(ninjaName))
				{
					current.SetData(tokenIn);
					return;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Debug.LogError((object)"No Matching Ninja!!");
	}

	public bool GetMatchingNinjaIsUnlocked(string ninjaName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UserDataElement current = enumerator.Current;
				if (current.IsKeyMatching(ninjaName))
				{
					return (bool)current.GetData(typeof(bool));
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Debug.LogError((object)"No Matching Ninja!!");
		return false;
	}

	public void SetMatchingNinjaIsUnlocked(string ninjaName, bool isUnlocked)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UserDataElement current = enumerator.Current;
				if (current.IsKeyMatching(ninjaName))
				{
					current.SetData(isUnlocked);
					return;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Debug.LogError((object)"No Matching Ninja!!");
	}

	public void AddCoin(int coinIn)
	{
		Coin += coinIn;
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getCoin, coinIn);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnTotalCoinChangedEvent());
	}

	public void IncreamentKill()
	{
		Kill++;
	}

	public void SpendCoin(int coinIn)
	{
		Coin += coinIn;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnTotalCoinChangedEvent());
	}

	public int GetUpgradeLevel(ShopItem.Type itemTypeIn)
	{
		switch (itemTypeIn)
		{
		case ShopItem.Type.flameUpgrade:
			return FlameUpgradeLevel;
		case ShopItem.Type.goldUpgrade:
			return GoldUpgradeLevel;
		case ShopItem.Type.thunderUpgrade:
			return ThunderUpgradeLevel;
		case ShopItem.Type.windUpgrade:
			return WindUpgradeLevel;
		default:
			Debug.LogError((object)"Invalid ShopItem Type!!");
			return -1;
		}
	}

	public int GetUpgradeLevel(ScrollElement elementIn)
	{
		switch (elementIn)
		{
		case ScrollElement.Flame:
			return FlameUpgradeLevel;
		case ScrollElement.Gold:
			return GoldUpgradeLevel;
		case ScrollElement.Thunder:
			return ThunderUpgradeLevel;
		case ScrollElement.Wind:
			return WindUpgradeLevel;
		default:
			Debug.LogError((object)"Invalid Scroll Element!!");
			return -1;
		}
	}

	private void OnApplicationPause()
	{
		FlushAllUserData();
	}

	private void OnApplicationQuit()
	{
		FlushAllUserData();
	}

	private void OnPause()
	{
		FlushAllUserData();
	}

	private void OnHome()
	{
		FlushAllUserData();
	}

	private void OnGameOver(GameOverResult result)
	{
		FlushAllUserData();
	}

	private void OnPurchaseSuccess(ShopItem.Type itemType)
	{
		FlushAllUserData();
	}

	private void OnCoinPurchaseSuccess()
	{
		FlushAllUserData();
	}

	private void FlushAllUserData()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = allUserData.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UserDataElement current = enumerator.Current;
				current.Flush();
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}
}
