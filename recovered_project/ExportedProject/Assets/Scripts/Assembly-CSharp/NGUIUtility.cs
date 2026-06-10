using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class NGUIUtility
{
	public static GameObject AddChild(GameObject parentIn, GameObject prefabIn)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = PoolManager.Spawn(prefabIn);
		if ((Object)(object)val != (Object)null && (Object)(object)parentIn != (Object)null)
		{
			Transform transform = val.transform;
			transform.parent = parentIn.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			val.layer = parentIn.layer;
		}
		return val;
	}

	public static void DestroyWhenTweenFinished(GameObject goIn, UITweener tweenIn)
	{
		PoolManager.Instance.DespawnAfterDelay(goIn, tweenIn.delay + tweenIn.duration);
	}

	public static void Destroy(GameObject goIn)
	{
		PoolManager.Despawn(goIn);
	}
}
