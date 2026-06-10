using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShurikenRange : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterShurikenRange", (SendMessageOptions)1);
	}

	private void OnTriggerExit(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnExitShurikenRange", (SendMessageOptions)1);
	}

	private void OnFinishBuff(ScrollElement element)
	{
		if (element == ScrollElement.Wind)
		{
			PoolManager.Despawn(((Component)this).gameObject);
		}
	}
}
