using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RandomSpawn : MonoBehaviour
{
	public List<GameObject> spawnObjectPrefabs;

	protected GameObject spawnedObjectInst;

	private void OnGetObjectFromPool()
	{
		spawnedObjectInst = PoolManager.SpawnAndAttachToParent(spawnObjectPrefabs[UnityEngine.Random.Range(0, spawnObjectPrefabs.Count)], ((Component)this).gameObject);
	}

	private void OnPutObjectIntoPool()
	{
		if ((Object)(object)spawnedObjectInst != (Object)null)
		{
			PoolManager.Despawn(spawnedObjectInst);
			spawnedObjectInst = null;
		}
	}

	private void OnObjectDespawned()
	{
		spawnedObjectInst = null;
	}
}
