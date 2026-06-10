using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FrameControl : MonoBehaviour
{
	private void Start()
	{
		if (Utility.IsGoodPerformance())
		{
			Application.targetFrameRate = 60;
		}
		else
		{
			Application.targetFrameRate = 60;
		}
	}

	private void Update()
	{
	}
}
