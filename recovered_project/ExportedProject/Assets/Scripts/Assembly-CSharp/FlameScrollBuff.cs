using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FlameScrollBuff : ScrollBuff
{
	public GameObject attackEffect;

	private void OnKilledZombie(ZombieType zType)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Utility.IsGoodPerformance())
		{
			PoolManager.Spawn(attackEffect, ((Component)this).transform.position);
		}
		((Component)((Component)this).transform.root).BroadcastMessage("OnKilledZombieWithFlame");
	}
}
