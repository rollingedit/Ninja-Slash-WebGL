using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Bamboo : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterBamboo", (object)((Component)this).gameObject, (SendMessageOptions)1);
	}
}
