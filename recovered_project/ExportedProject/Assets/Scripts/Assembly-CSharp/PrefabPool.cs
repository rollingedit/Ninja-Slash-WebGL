using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class PrefabPool
{
	public PoolObject Prefab;

	public int PreAlloc = 8;

	public int AllocBlock = 1;

	public bool HardLimit;

	public int Limit = 8;

	public bool Cull;

	public int CullAbove = 8;

	public float CullDelay = 10f;

	public bool ActivateRecursively;

	private Stack<GameObject> Pool;

	private float TimeOfLastCull = -3.4028235E+38f;

	private int SpawnCount;

	public void Awake()
	{
		Prefab.PrefabName = ((Object)((Component)Prefab).gameObject).name;
		Pool = new Stack<GameObject>(PreAlloc);
		Allocate(PreAlloc);
	}

	private void Allocate(int count)
	{
		if (HardLimit && Pool.Count + count > Limit)
		{
			count = Limit - Pool.Count;
		}
		for (int i = 0; i < count; i++)
		{
			Object obj = Object.Instantiate((Object)(object)((Component)Prefab).gameObject);
			GameObject val = (GameObject)(object)((obj is GameObject) ? obj : null);
			((Object)val).name = ((Object)val).name + i;
			Pool.Push(val);
		}
	}

	public GameObject Pop()
	{
		if (HardLimit && SpawnCount >= Limit)
		{
			return null;
		}
		if (Pool.Count > 0)
		{
			SpawnCount++;
			return Pool.Pop();
		}
		Allocate(AllocBlock);
		return Pop();
	}

	public void Push(GameObject go)
	{
		if (!HardLimit || Pool.Count < Limit)
		{
			SpawnCount = Mathf.Max(SpawnCount - 1, 0);
			Pool.Push(go);
		}
	}

	public void Poll()
	{
		if (Cull && Pool.Count > CullAbove && Time.time > TimeOfLastCull + CullDelay)
		{
			TimeOfLastCull = Time.time;
			for (int i = CullAbove; i <= Pool.Count; i++)
			{
				Object.Destroy((Object)(object)Pool.Pop());
			}
		}
	}

	public GameObject Spawn()
	{
		GameObject val = Pop();
		if ((Object)(object)val != (Object)null)
		{
			val.GetComponent<PoolObject>().OnSpawn(ActivateRecursively);
		}
		return val;
	}

	public GameObject Spawn(Vector3 pos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Pop();
		if ((Object)(object)val != (Object)null)
		{
			val.transform.position = pos;
			val.GetComponent<PoolObject>().OnSpawn(ActivateRecursively);
		}
		return val;
	}

	public void Despawn(GameObject go)
	{
		PoolObject component = go.GetComponent<PoolObject>();
		if (!((Object)(object)component == (Object)null) && !(component.PrefabName != Prefab.PrefabName))
		{
			// PORT FIX: pushing an object that is already in the pool would duplicate the
			// stack entry, making later spawns hand out the same instance twice (stacked
			// enemies / vanishing blocks). The original code pushed unconditionally.
			if (!component.IsSpawned)
			{
				return;
			}
			component.OnDespawn();
			Push(go);
		}
	}
}
