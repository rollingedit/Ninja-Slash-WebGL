using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RotateSample : MonoBehaviour
{
	private void Start()
	{
		iTween.RotateBy(((Component)this).gameObject, iTween.Hash("x", 0.25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 0.4));
	}
}
