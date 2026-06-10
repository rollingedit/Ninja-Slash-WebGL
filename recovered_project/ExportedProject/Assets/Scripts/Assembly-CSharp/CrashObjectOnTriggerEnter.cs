using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CrashObjectOnTriggerEnter : MonoBehaviour
{
	public GameObject prefab;

	public GameObject prefabLow;

	public bool ShurikenImmune;

	public bool BoomerImmune;

	private void Start()
	{
		if ((Object)(object)prefabLow == (Object)null)
		{
			prefabLow = prefab;
		}
	}

	private void OnCrashedByPlayer(Vector3 velocity)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		ChangePrefab(velocity);
	}

	private void OnCrashedByBoomer(Vector3 velocity)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (!BoomerImmune)
		{
			if (Utility.IsGoodPerformance())
			{
				ChangePrefab(velocity);
				return;
			}
			PoolManager.Despawn(((Component)this).gameObject);
			((Component)this).gameObject.SendMessage("OnZombieCrashed", (SendMessageOptions)1);
		}
	}

	private void OnHitByShuriken(Vector3 velocity)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!ShurikenImmune)
		{
			((Component)this).BroadcastMessage("OnCrashedByShuriken", (SendMessageOptions)1);
			ChangePrefab(velocity);
		}
	}

	private void ChangePrefab(Vector3 velocity)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = ((!Utility.IsGoodPerformance()) ? prefabLow : prefab);
		if ((Object)(object)val != (Object)null)
		{
			GameObject val2 = PoolManager.Spawn(val, ((Component)this).transform.position);
			val2.SendMessage("OnZombieCrashed", (SendMessageOptions)1);
			val2.SendMessage("OnApplyForce", (object)velocity, (SendMessageOptions)1);
		}
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
