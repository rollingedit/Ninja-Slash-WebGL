using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ThunderScrollBuff : ScrollBuff
{
	public GameObject attackEffect;

	private void OnKilledZombie(ZombieType zType)
	{
		if (Utility.IsGoodPerformance())
		{
			PoolManager.SpawnAndAttachToParent(attackEffect, ((Component)this).gameObject);
		}
		((Component)((Component)this).transform.root).BroadcastMessage("OnKilledZombieWithThunder");
	}
}
