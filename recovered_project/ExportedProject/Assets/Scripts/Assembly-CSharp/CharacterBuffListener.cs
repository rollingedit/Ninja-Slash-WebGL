using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterBuffListener : MonoBehaviour
{
	public Buff flameBuff;

	public Buff goldBuff;

	public Buff thunderBuff;

	public Buff windBuff;

	private Dictionary<ScrollElement, bool> buffState;

	private Dictionary<ScrollElement, Buff> buffStore;

	private void Start()
	{
		buffState = new Dictionary<ScrollElement, bool>();
		buffState.Add(ScrollElement.Flame, false);
		buffState.Add(ScrollElement.Wind, false);
		buffState.Add(ScrollElement.Gold, false);
		buffState.Add(ScrollElement.Thunder, false);
		buffStore = new Dictionary<ScrollElement, Buff>();
		buffStore.Add(ScrollElement.Flame, flameBuff);
		buffStore.Add(ScrollElement.Gold, goldBuff);
		buffStore.Add(ScrollElement.Thunder, thunderBuff);
		buffStore.Add(ScrollElement.Wind, windBuff);
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (buffState[elementIn])
		{
			((Component)this).BroadcastMessage("OnScrollBuffAlreadyExist", (object)elementIn);
			return;
		}
		buffState[elementIn] = true;
		GetBuffFromPool(buffStore[elementIn]);
	}

	private void GetBuffFromPool(Buff buffIn)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = PoolManager.Spawn(buffIn.buffPrefab);
		val.transform.parent = ((Component)this).transform;
		val.transform.localPosition = Vector3.zero;
		val.SendMessage("StartBuff", (object)buffIn);
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		buffState[elementIn] = false;
	}
}
