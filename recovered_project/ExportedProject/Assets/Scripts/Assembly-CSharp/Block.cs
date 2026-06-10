using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Block : MonoBehaviour
{
	public void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player") && ((Component)collider).gameObject.GetComponent<CharacterTrapListener>().isBlockTriggerCheck)
		{
			PoolManager.Despawn(((Component)this).gameObject);
		}
	}
}
