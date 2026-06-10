using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class ShopItem
{
	public enum Type
	{
		multiplierBooster = 0,
		shield = 1,
		luckyBox = 2,
		skipMission = 3,
		flameUpgrade = 4,
		goldUpgrade = 5,
		thunderUpgrade = 6,
		windUpgrade = 7
	}

	[SerializeField]
	public Type itemType;

	[SerializeField]
	public int price;
}
