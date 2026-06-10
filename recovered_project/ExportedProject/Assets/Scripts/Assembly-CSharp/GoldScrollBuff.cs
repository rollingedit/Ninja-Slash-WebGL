using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class GoldScrollBuff : ScrollBuff
{
	public GameObject magnetPrefab;

	public override void StartEffect()
	{
		PoolManager.SpawnAndAttachToParent(magnetPrefab, ((Component)this).gameObject);
	}
}
