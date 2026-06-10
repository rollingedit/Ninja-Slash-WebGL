using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CameraAnimation : MonoBehaviour
{
	private void OnDead()
	{
		GetComponent<Animation>().Play("vibrate");
	}

	private void OnStartRush()
	{
		if (Utility.IsGoodPerformance())
		{
			GetComponent<Animation>().Play("vibrate_berserker");
		}
	}
}
