using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Shield : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessageUpwards("OnEnterShieldRange", (SendMessageOptions)1);
	}

	private void OnTriggerExit(Collider collider)
	{
		((Component)collider).gameObject.SendMessageUpwards("OnExitShieldRange", (SendMessageOptions)1);
	}
}
