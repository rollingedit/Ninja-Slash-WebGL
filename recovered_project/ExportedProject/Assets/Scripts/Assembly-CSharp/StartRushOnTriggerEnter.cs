using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class StartRushOnTriggerEnter : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			((Component)this).BroadcastMessage("OnStartRush", (object)((Component)collider).gameObject);
		}
	}
}
