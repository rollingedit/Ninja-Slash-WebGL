using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SpawnTrap : MonoBehaviour
{
	public GameObject normalProp;

	public GameObject trapProp;

	public AudioClip trapSound;

	private GameObject spawnedObjectInst;

	private void OnGetObjectFromPool()
	{
		spawnedObjectInst = PoolManager.SpawnAndAttachToParent(normalProp, ((Component)this).gameObject);
	}

	private void OnPutObjectIntoPool()
	{
		PoolManager.Despawn(spawnedObjectInst);
	}

	private void OnObjectDespawned()
	{
		spawnedObjectInst = null;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			PoolManager.Despawn(spawnedObjectInst);
			spawnedObjectInst = PoolManager.SpawnAndAttachToParent(trapProp, ((Component)this).gameObject);
			MonoSingleton<SoundManager>.instance.PlaySound(trapSound);
		}
	}
}
