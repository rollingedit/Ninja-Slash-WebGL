using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class LuckyBoxManager : MonoSingleton<LuckyBoxManager>
{
	public List<LuckyBoxItem> items;

	private float totalProbability;

	private void Start()
	{
		SetItemList();
	}

	private void SetItemList()
	{
		totalProbability = 0f;
		for (int i = 0; i < items.Count; i++)
		{
			if (((global::System.Enum)items[i].reward.type).ToString().Contains("Token_"))
			{
				Ninja ninjaByName = MonoSingleton<NinjaInfo>.instance.GetNinjaByName(((global::System.Enum)items[i].reward.type).ToString().Remove(0, 6));
				if (ninjaByName.IsNinjaUnlocked())
				{
					items.Remove(items[i]);
					i--;
					continue;
				}
			}
			totalProbability += items[i].probability;
		}
	}

	public void OnGetLuckyBox()
	{
		Reward itemIn = PickALuckyBoxItem();
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGetLuckyBoxEvent(itemIn));
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getLuckyBox);
		StoreItem(itemIn);
	}

	public void OnBuyLuckyBox()
	{
		Reward itemIn = PickALuckyBoxItem();
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnBuyLuckyBoxEvent(itemIn));
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.getLuckyBox);
		StoreCoin(itemIn);
		StoreItem(itemIn);
	}

	private Reward PickALuckyBoxItem()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		LuckyBoxItem luckyBoxItem = null;
		float num = 0f;
		float num2 = UnityEngine.Random.Range(0f, totalProbability);
		var enumerator = items.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LuckyBoxItem current = enumerator.Current;
				num += current.probability;
				if (num2 <= num)
				{
					luckyBoxItem = current;
					break;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		return luckyBoxItem.reward;
	}

	private void StoreCoin(Reward itemIn)
	{
		if (((global::System.Enum)itemIn.type).Equals((object)Reward.Type.Coin))
		{
			MonoSingleton<UserData>.instance.AddCoin(itemIn.quantity);
		}
	}

	private void StoreItem(Reward itemIn)
	{
		if (((global::System.Enum)itemIn.type).Equals((object)Reward.Type.MultiplierBooster))
		{
			MonoSingleton<UserData>.instance.MultiplierBoosterCount += itemIn.quantity;
		}
		else if (((global::System.Enum)itemIn.type).Equals((object)Reward.Type.Shield))
		{
			MonoSingleton<UserData>.instance.ShieldCount += itemIn.quantity;
		}
		else if (((global::System.Enum)itemIn.type).ToString().Contains("Token_"))
		{
			MonoSingleton<NinjaInfo>.instance.GetNinjaByName(((global::System.Enum)itemIn.type).ToString().Remove(0, 6)).curToken++;
		}
	}

	private void OnNinjaUnlocked(Ninja ninja)
	{
		SetItemList();
	}
}
