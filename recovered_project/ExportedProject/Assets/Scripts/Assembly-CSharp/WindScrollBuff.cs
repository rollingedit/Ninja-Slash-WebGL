using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class WindScrollBuff : ScrollBuff
{
	public GameObject shurikenPrefab;

	public GameObject shurikenRangePrefab;

	private int thrownShurikenCount;

	private List<GameObject> targets;

	private void Awake()
	{
		targets = new List<GameObject>();
	}

	public override void StartEffect()
	{
		PoolManager.SpawnAndAttachToParent(shurikenRangePrefab, ((Component)this).gameObject);
	}

	private void OnGetObjectFromPool()
	{
		thrownShurikenCount = 0;
	}

	private void OnPutObjectIntoPool()
	{
		targets.Clear();
	}

	private void OnShurikenPooled()
	{
		thrownShurikenCount--;
	}

	public override void DoUpdate()
	{
		if (thrownShurikenCount <= 0 && targets.Count != 0)
		{
			if (targets[0].activeSelf)
			{
				ThrowShuriken(targets[0]);
			}
			else
			{
				targets.RemoveAt(0);
			}
		}
	}

	private void OnShurikenRange(ShurikenRangeData dataIn)
	{
		if (dataIn.isEntered)
		{
			targets.Add(dataIn.gameObject);
		}
		else
		{
			targets.Remove(dataIn.gameObject);
		}
	}

	private void ThrowShuriken(GameObject go)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = PoolManager.Spawn(shurikenPrefab, ((Component)this).transform.position);
		val.SendMessage("OnTraceTarget", (object)go);
		((Component)((Component)this).transform.root).BroadcastMessage("OnThrowShuriken");
		thrownShurikenCount++;
	}
}
