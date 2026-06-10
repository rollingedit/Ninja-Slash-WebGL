using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialEndTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			((Component)this).SendMessageUpwards("OnExitBlock");
		}
	}
}
