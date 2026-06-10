using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TrapWall : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterTrapWall", (SendMessageOptions)1);
	}
}
