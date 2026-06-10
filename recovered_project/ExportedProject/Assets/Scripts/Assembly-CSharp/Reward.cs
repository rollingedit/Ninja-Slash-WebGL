using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class Reward
{
	public enum Type
	{
		Coin = 0,
		MultiplierBooster = 1,
		Shield = 2,
		Token_Shiro = 3,
		Token_Naomi = 4,
		Token_Daigo = 5
	}

	[SerializeField]
	public Type type;

	[SerializeField]
	public int quantity;

	public string iconSpriteName
	{
		get
		{
			if (type == Type.Coin)
			{
				return "icon_coin";
			}
			if (type == Type.MultiplierBooster)
			{
				return "icon_headstart";
			}
			if (type == Type.Shield)
			{
				return "icon_shadowcloak";
			}
			if (type == Type.Token_Shiro)
			{
				return "token1";
			}
			if (type == Type.Token_Naomi)
			{
				return "token4";
			}
			if (type == Type.Token_Daigo)
			{
				return "token5";
			}
			Debug.LogError((object)"Invalid Reward Type!!");
			return string.Empty;
		}
	}

	public Reward(Type typeIn, int quantityIn)
	{
		type = typeIn;
		quantity = quantityIn;
	}
}
