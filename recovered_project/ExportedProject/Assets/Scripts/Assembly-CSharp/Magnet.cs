using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Magnet : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterMagnetRange", (SendMessageOptions)1);
	}

	private void OnFinishBuff(ScrollElement element)
	{
		if (element == ScrollElement.Gold)
		{
			PoolManager.Despawn(((Component)this).gameObject);
		}
	}
}
