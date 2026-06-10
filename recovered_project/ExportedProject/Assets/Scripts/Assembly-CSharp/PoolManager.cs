using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PoolManager : MonoBehaviour
{
	private static PoolManager s_instance;

	public List<PrefabPool> PrefabPoolCollection;

	private static Dictionary<string, PrefabPool> Pools = new Dictionary<string, PrefabPool>();

	public static PoolManager Instance
	{
		get
		{
			return s_instance;
		}
		private set
		{
			s_instance = value;
		}
	}

	private void Awake()
	{
		if ((Object)(object)Instance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
			return;
		}
		Instance = this;
		InitializePrefabPools();
	}

	private void OnLevelWasLoaded()
	{
		Pools.Clear();
	}

	private void InitializePrefabPools()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (PrefabPoolCollection == null)
		{
			return;
		}
		var enumerator = PrefabPoolCollection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PrefabPool current = enumerator.Current;
				if (current != null && !((Object)(object)current.Prefab == (Object)null))
				{
					current.Awake();
					Pools.Add(current.Prefab.PrefabName, current);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	public static bool PoolExists(string name)
	{
		return Pools.ContainsKey(name);
	}

	public static bool PoolExists(GameObject go)
	{
		PoolObject component = go.GetComponent<PoolObject>();
		return (Object)(object)component != (Object)null && Pools.ContainsKey(component.PrefabName);
	}

	public static GameObject Spawn(string name)
	{
		PrefabPool prefabPool = default(PrefabPool);
		if (Pools.TryGetValue(name, out prefabPool))
		{
			return prefabPool.Spawn();
		}
		return null;
	}

	public static GameObject Spawn(GameObject go)
	{
		PoolObject component = go.GetComponent<PoolObject>();
		GameObject val = null;
		if ((Object)(object)component != (Object)null && PoolExists(component.PrefabName))
		{
			val = Pools[component.PrefabName].Spawn();
		}
		else
		{
			Object obj = Object.Instantiate((Object)(object)go);
			val = (GameObject)(object)((obj is GameObject) ? obj : null);
			Debug.LogWarning((object)string.Concat((object)go, (object)" is not pooled"));
		}
		if ((Object)(object)val != (Object)null)
		{
			val.BroadcastMessage("OnGetObjectFromPool", (SendMessageOptions)1);
		}
		else
		{
			Debug.LogError((object)string.Concat((object)go, (object)"is not found"));
		}
		return val;
	}

	public static GameObject Spawn(GameObject go, Vector3 wantedPosition)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		PoolObject component = go.GetComponent<PoolObject>();
		GameObject val = null;
		if ((Object)(object)component != (Object)null && PoolExists(component.PrefabName))
		{
			val = Pools[component.PrefabName].Spawn(wantedPosition);
		}
		else
		{
			Object obj = Object.Instantiate((Object)(object)go, wantedPosition, Quaternion.identity);
			val = (GameObject)(object)((obj is GameObject) ? obj : null);
			Debug.LogWarning((object)string.Concat((object)go, (object)" is not pooled"));
		}
		if ((Object)(object)val != (Object)null)
		{
			val.BroadcastMessage("OnGetObjectFromPool", (SendMessageOptions)1);
		}
		else
		{
			Debug.LogError((object)string.Concat((object)go, (object)" is not found"));
		}
		return val;
	}

	public static GameObject SpawnAndAttachToParent(GameObject go, GameObject parent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = Spawn(go, parent.transform.position);
		val.transform.rotation = parent.transform.rotation;
		val.transform.parent = parent.transform;
		return val;
	}

	public void DespawnAfterDelay(GameObject go, float delay)
	{
		((MonoBehaviour)this).StartCoroutine(WaitAndDespawn(go, delay));
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndDespawn(GameObject go, float delay)
	{
		yield return (object)new WaitForSeconds(delay);
		Despawn(go);
	}

	public static void Despawn(GameObject go)
	{
		if (!((Object)(object)go == (Object)null))
		{
			go.BroadcastMessage("OnPutObjectIntoPool", (SendMessageOptions)1);
			PoolObject component = go.GetComponent<PoolObject>();
			if ((Object)(object)component == (Object)null || !PoolExists(component.PrefabName))
			{
				Object.Destroy((Object)(object)go);
			}
			else
			{
				Pools[component.PrefabName].Despawn(go);
			}
		}
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = PrefabPoolCollection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PrefabPool current = enumerator.Current;
				current.Poll();
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}
}
