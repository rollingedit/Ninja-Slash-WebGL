using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MoveSample : MonoBehaviour
{
	private void Start()
	{
		iTween.MoveTo(((Component)this).gameObject, iTween.Hash("x", 1.7, "easeType", "linear", "time", 2));
	}
}
