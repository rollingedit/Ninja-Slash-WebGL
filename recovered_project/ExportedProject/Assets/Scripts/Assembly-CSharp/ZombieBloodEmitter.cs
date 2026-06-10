using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ZombieBloodEmitter : MonoBehaviour
{
	public GameObject bloodEffect;

	private void OnZombieCrashed()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Utility.IsGoodPerformance())
		{
			PoolManager.Spawn(bloodEffect, ((Component)this).transform.position);
		}
	}
}
