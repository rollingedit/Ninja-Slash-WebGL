using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterShieldListener : MonoBehaviour
{
	public GameObject shieldPrefab;

	public float shieldTime;

	public float shieldCoolTime;

	public GameObject shieldEffectPrefab;

	private GameObject shieldEffectObject;

	private GameObject shieldObject;

	private bool isShieldOn;

	private bool isShieldCoolTime;

	private float leftTime;

	private void OnShieldStart()
	{
		shieldObject = PoolManager.SpawnAndAttachToParent(shieldPrefab, ((Component)this).gameObject);
		if (Utility.IsGoodPerformance())
		{
			shieldEffectObject = PoolManager.SpawnAndAttachToParent(shieldEffectPrefab, ((Component)this).gameObject);
		}
		isShieldOn = true;
		leftTime = shieldTime;
	}

	private void Update()
	{
		if (isShieldOn)
		{
			leftTime = Mathf.Clamp(leftTime - Time.deltaTime, 0f, leftTime);
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnUpdateTimeEvent(new UpdateTimeData(leftTime, shieldTime, UpdateTimeTarget.shield)));
			if (leftTime <= 0f)
			{
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.finish));
				StartShieldCoolTime();
			}
		}
		else if (isShieldCoolTime)
		{
			leftTime = Mathf.Clamp(leftTime - Time.deltaTime, 0f, leftTime);
			if (leftTime <= 0f)
			{
				FinishShieldCoolTime();
			}
		}
	}

	private void OnShieldUsed()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.finish));
		StartShieldCoolTime();
	}

	private void OnShieldFinish()
	{
		isShieldOn = false;
		PoolManager.Despawn(shieldObject);
		if ((Object)(object)shieldEffectObject != (Object)null)
		{
			PoolManager.Despawn(shieldEffectObject);
		}
	}

	private void StartShieldCoolTime()
	{
		isShieldCoolTime = true;
		leftTime = shieldCoolTime;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.coolTimeStart));
	}

	private void FinishShieldCoolTime()
	{
		isShieldCoolTime = false;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.coolTimeFinish));
	}

	private void Reset()
	{
		leftTime = 0f;
		if (isShieldOn)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnShieldEvent(ShieldState.finish));
		}
		else if (isShieldCoolTime)
		{
			FinishShieldCoolTime();
		}
	}

	private void OnDead()
	{
		Reset();
	}

	private void OnPutObjectIntoPool()
	{
		Reset();
	}
}
