using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class GateDoor : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.BroadcastMessage("OnEnterGateDoor", (object)((Component)this).gameObject, (SendMessageOptions)1);
	}
}
