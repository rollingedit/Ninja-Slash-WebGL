using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterCoinListener : MonoBehaviour
{
	public GameObject coinGetEffect1;

	public GameObject coinGetEffect2;

	private int curCoinIndex;

	private bool isGoldBuffOn;

	public CharacterRecord characterRecord;

	private void OnGetObjectFromPool()
	{
		curCoinIndex = 0;
	}

	private void OnEnterCoin(GameObject coin)
	{
		((Component)this).SendMessage("OnGetCoin");
		coin.SendMessage("OnGetByPlayer");
	}

	private void OnGetCoin()
	{
		characterRecord.AddCoin((!isGoldBuffOn) ? 1 : 2);
		if (Utility.IsGoodPerformance())
		{
			PoolManager.SpawnAndAttachToParent((curCoinIndex != 0) ? coinGetEffect2 : coinGetEffect1, ((Component)this).gameObject);
			curCoinIndex = (curCoinIndex + 1) % 2;
		}
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = true;
		}
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		if (((global::System.Enum)elementIn).Equals((object)ScrollElement.Gold))
		{
			isGoldBuffOn = false;
		}
	}

	private void OnComboBonus(ComboBonus bonusIn)
	{
		if (bonusIn.reward.type == Reward.Type.Coin)
		{
			characterRecord.AddCoin(bonusIn.reward.quantity);
		}
	}

	private void OnGetLuckyBox(Reward rewardIn)
	{
		if (rewardIn.type == Reward.Type.Coin)
		{
			characterRecord.AddCoin(rewardIn.quantity);
		}
	}
}
