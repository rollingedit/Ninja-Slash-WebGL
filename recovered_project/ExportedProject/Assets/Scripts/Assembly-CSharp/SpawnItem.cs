using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SpawnItem : MonoBehaviour
{
	private GameObject spawnedObjectInst;

	private void OnGetObjectFromPool()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		spawnedObjectInst = MonoSingleton<ItemSpawnManager>.instance.SpawnRandomItem();
		if ((Object)(object)spawnedObjectInst != (Object)null)
		{
			spawnedObjectInst.transform.position = ((Component)this).transform.position;
			spawnedObjectInst.transform.parent = ((Component)this).transform;
		}
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
