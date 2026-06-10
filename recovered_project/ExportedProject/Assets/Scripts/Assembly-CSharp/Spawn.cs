using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Spawn : MonoBehaviour
{
	public GameObject spawnObjectPrefab;

	protected GameObject spawnedObjectInst;

	private void OnGetObjectFromPool()
	{
		spawnedObjectInst = PoolManager.SpawnAndAttachToParent(spawnObjectPrefab, ((Component)this).gameObject);
	}

	private void OnPutObjectIntoPool()
	{
		PoolManager.Despawn(spawnedObjectInst);
		spawnedObjectInst = null;
	}

	private void OnObjectDespawned()
	{
		spawnedObjectInst = null;
	}
}
