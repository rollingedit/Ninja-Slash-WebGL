using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ScrollBuff : MonoBehaviour
{
	public ScrollElement element;

	public UpdateTimeTarget target;

	protected float totalTime;

	protected float leftTime;

	protected GameObject effectObject;

	public void StartBuff(Buff buffIn)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		StartEffect();
		totalTime = buffIn.defaultTime + (float)MonoSingleton<UserData>.instance.GetUpgradeLevel(element) * buffIn.timePerUpgrade;
		leftTime = totalTime;
		GameObject val = ((!Utility.IsGoodPerformance()) ? buffIn.effectPrefabForLowPerformance : buffIn.effectPrefab);
		if ((Object)(object)val != (Object)null)
		{
			effectObject = PoolManager.SpawnAndAttachToParent(buffIn.effectPrefab, ((Component)this).gameObject);
			effectObject.transform.localScale = Vector3.one;
		}
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnStartBuffEvent(element));
		if (Utility.IsGoodPerformance())
		{
			PoolManager.SpawnAndAttachToParent(buffIn.letterPrefab, ((Component)this).gameObject);
		}
	}

	public void Update()
	{
		if (leftTime > 0f)
		{
			leftTime -= Time.deltaTime;
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnUpdateTimeEvent(new UpdateTimeData(leftTime, totalTime, target)));
			if (leftTime <= 0f)
			{
				FinishEffect();
				PoolManager.Despawn(((Component)this).gameObject);
			}
		}
		DoUpdate();
	}

	public void OnDead()
	{
		FinishEffect();
		PoolManager.Despawn(((Component)this).gameObject);
	}

	private void OnPlayerPooled()
	{
		FinishEffect();
		PoolManager.Despawn(((Component)this).gameObject);
	}

	public void OnScrollBuffAlreadyExist(ScrollElement elementIn)
	{
		if (element == elementIn)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnStartBuffEvent(element));
			leftTime = totalTime;
		}
	}

	public virtual void StartEffect()
	{
	}

	public virtual void DoUpdate()
	{
	}

	public void FinishEffect()
	{
		((Component)((Component)this).transform.root).BroadcastMessage("OnFinishBuff", (object)element);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnFinishBuffEvent(element));
		if ((Object)(object)effectObject != (Object)null)
		{
			PoolManager.Despawn(effectObject);
		}
	}
}
