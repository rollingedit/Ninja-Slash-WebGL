using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SpawnPlayer : MonoBehaviour
{
	public GameObject playerPrefab;

	private GameObject playerObjectInst;

	public Vector3 spawnPosition;

	private void Start()
	{
		SpawnObject();
	}

	private void SpawnObject()
	{
		playerObjectInst = PoolManager.SpawnAndAttachToParent(playerPrefab, ((Component)this).gameObject);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnSpawnObjectEvent(playerObjectInst));
	}

	private void OnRestart()
	{
		// PORT FIX: the original assumed the player was already despawned by the death
		// sequence. If a run ends while the player object is still spawned (e.g. it never
		// died because it escaped the kill volumes), the hard-limited "player" pool returns
		// null on the next spawn and the game soft-locks. Despawn the lingering instance
		// first, exactly like OnHome() does.
		if ((Object)(object)playerObjectInst != (Object)null)
		{
			PoolObject poolObject = playerObjectInst.GetComponent<PoolObject>();
			if ((Object)(object)poolObject != (Object)null && poolObject.IsSpawned)
			{
				PoolManager.Despawn(playerObjectInst);
			}
		}
		SpawnObject();
		playerObjectInst.BroadcastMessage("OnRestart", (SendMessageOptions)1);
	}

	private void OnHome()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		PoolManager.Despawn(playerObjectInst);
		playerObjectInst.transform.position = Vector3.zero;
		SpawnObject();
	}

	private void OnShieldUsed()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		playerObjectInst.transform.position = ((Component)this).transform.position;
	}

	private void OnTutorialGameOver()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		PoolManager.Despawn(playerObjectInst);
		playerObjectInst.transform.position = Vector3.zero;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnRestartEvent());
		playerObjectInst.transform.position = spawnPosition;
	}

	private void OnEnterTutorialBlock(GameObject blockObject)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		spawnPosition = blockObject.transform.TransformPoint(0f, 3f, 10f);
	}
}
