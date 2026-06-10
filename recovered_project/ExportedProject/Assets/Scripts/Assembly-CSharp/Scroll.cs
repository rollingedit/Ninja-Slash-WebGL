using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Scroll : MonoBehaviour
{
	public ScrollElement element;

	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			((Component)collider).gameObject.BroadcastMessage("OnGetScroll", (object)element);
			PoolManager.Despawn(((Component)this).gameObject);
		}
	}
}
