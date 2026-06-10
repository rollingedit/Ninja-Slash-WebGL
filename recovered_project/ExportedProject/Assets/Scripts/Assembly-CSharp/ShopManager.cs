using System.Collections.Generic;

public class ShopManager : MonoSingleton<ShopManager>
{
	public ShopItem[] shopItems;

	public int[] upgradeCosts;

	public int increasedCostPerMission;

	private Dictionary<ShopItem.Type, int> shopItemDict;

	private void Start()
	{
		shopItemDict = new Dictionary<ShopItem.Type, int>();
		ShopItem[] array = shopItems;
		foreach (ShopItem shopItem in array)
		{
			shopItemDict.Add(shopItem.itemType, shopItem.price);
		}
		SetSkipMissionPrice();
		SetUpgradePrice(ShopItem.Type.flameUpgrade, MonoSingleton<UserData>.instance.FlameUpgradeLevel);
		SetUpgradePrice(ShopItem.Type.goldUpgrade, MonoSingleton<UserData>.instance.GoldUpgradeLevel);
		SetUpgradePrice(ShopItem.Type.thunderUpgrade, MonoSingleton<UserData>.instance.ThunderUpgradeLevel);
		SetUpgradePrice(ShopItem.Type.windUpgrade, MonoSingleton<UserData>.instance.WindUpgradeLevel);
	}

	private void OnPurchase(ShopItem.Type itemType)
	{
		if (MonoSingleton<UserData>.instance.Coin >= 0)
		{
			MonoSingleton<UserData>.instance.SpendCoin(shopItemDict[itemType]);
			switch (itemType)
			{
			case ShopItem.Type.flameUpgrade:
			case ShopItem.Type.goldUpgrade:
			case ShopItem.Type.thunderUpgrade:
			case ShopItem.Type.windUpgrade:
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnPurchaseSuccessEvent(itemType));
				SetUpgradePrice(itemType, MonoSingleton<UserData>.instance.GetUpgradeLevel(itemType));
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangePriceEvent());
				break;
			case ShopItem.Type.skipMission:
				if (MonoSingleton<UserData>.instance.MissionProgressId < MonoSingleton<MissionManager>.instance.missions.Count)
				{
					MonoSingleton<EventManager>.instance.TriggerEvent(new OnPurchaseSuccessEvent(itemType));
					SetSkipMissionPrice();
					MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangePriceEvent());
				}
				break;
			default:
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnPurchaseSuccessEvent(itemType));
				break;
			}
		}
		else
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnPurchaseFailEvent());
		}
	}

	private void SetSkipMissionPrice()
	{
		if (MonoSingleton<MissionManager>.instance.GetCurrentMission().type != Mission.Type.allClear)
		{
			shopItemDict[ShopItem.Type.skipMission] = increasedCostPerMission * MonoSingleton<MissionManager>.instance.GetCurrentMission().id;
		}
		else
		{
			shopItemDict[ShopItem.Type.skipMission] = 2147483647;
		}
	}

	private void SetUpgradePrice(ShopItem.Type itemTypeIn, int levelIn)
	{
		if (levelIn < upgradeCosts.Length)
		{
			shopItemDict[itemTypeIn] = upgradeCosts[levelIn];
		}
		else
		{
			shopItemDict[itemTypeIn] = 2147483647;
		}
	}

	public int GetItemPrice(ShopItem.Type itemType)
	{
		return shopItemDict[itemType];
	}

	private void OnPurchaseSuccess(ShopItem.Type itemType)
	{
		switch (itemType)
		{
		case ShopItem.Type.multiplierBooster:
			MonoSingleton<UserData>.instance.MultiplierBoosterCount++;
			break;
		case ShopItem.Type.shield:
			MonoSingleton<UserData>.instance.ShieldCount++;
			break;
		case ShopItem.Type.luckyBox:
			MonoSingleton<LuckyBoxManager>.instance.OnBuyLuckyBox();
			break;
		case ShopItem.Type.skipMission:
			MonoSingleton<MissionManager>.instance.CompleteCurrentMission();
			break;
		case ShopItem.Type.flameUpgrade:
			MonoSingleton<UserData>.instance.FlameUpgradeLevel++;
			break;
		case ShopItem.Type.goldUpgrade:
			MonoSingleton<UserData>.instance.GoldUpgradeLevel++;
			break;
		case ShopItem.Type.thunderUpgrade:
			MonoSingleton<UserData>.instance.ThunderUpgradeLevel++;
			break;
		case ShopItem.Type.windUpgrade:
			MonoSingleton<UserData>.instance.WindUpgradeLevel++;
			break;
		}
	}

	private void OnMissionComplete()
	{
		SetSkipMissionPrice();
	}
}
