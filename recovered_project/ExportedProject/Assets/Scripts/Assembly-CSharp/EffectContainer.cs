using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class EffectContainer : MonoBehaviour
{
	private int childCount;

	private void OnGetObjectFromPool()
	{
		childCount = ((Component)this).transform.childCount;
	}

	private void OnFinishEffect()
	{
		childCount--;
		if (childCount == 0)
		{
			PoolManager.Despawn(((Component)this).gameObject);
		}
	}
}
