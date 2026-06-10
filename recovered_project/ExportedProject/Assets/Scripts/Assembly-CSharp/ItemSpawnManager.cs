using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ItemSpawnManager : MonoSingleton<ItemSpawnManager>
{
	[Serializable]
	public class Item
	{
		[SerializeField]
		public GameObject itemPrefab;

		[SerializeField]
		public float probability;
	}

	public List<Item> itemList;

	private void Start()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		var enumerator = itemList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Item current = enumerator.Current;
				num += current.probability;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		if (num > 1f)
		{
			Debug.LogError((object)"Sum of item spawn probability is more than 1!");
		}
	}

	public GameObject SpawnRandomItem()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		float num = UnityEngine.Random.Range(0f, 1f);
		float num2 = 0f;
		var enumerator = itemList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Item current = enumerator.Current;
				num2 += current.probability;
				if (num < num2)
				{
					return PoolManager.Spawn(current.itemPrefab);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		return null;
	}
}
