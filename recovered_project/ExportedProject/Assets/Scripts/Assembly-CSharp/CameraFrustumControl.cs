using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CameraFrustumControl : MonoBehaviour
{
	public float cameraFarLowPerformance;

	private float originalFar;

	private float originalFogStart;

	private float originalFogEnd;

	private void Start()
	{
		if (!Utility.IsGoodPerformance())
		{
			originalFar = Camera.main.farClipPlane;
			originalFogStart = RenderSettings.fogStartDistance;
			originalFogEnd = RenderSettings.fogEndDistance;
			Camera.main.farClipPlane = cameraFarLowPerformance;
			RenderSettings.fogEndDistance = cameraFarLowPerformance;
			RenderSettings.fogStartDistance = cameraFarLowPerformance * 0.5f;
		}
	}

	private void OnApplicationQuit()
	{
		if (!Utility.IsGoodPerformance())
		{
			Camera.main.farClipPlane = originalFar;
			RenderSettings.fogStartDistance = originalFogStart;
			RenderSettings.fogEndDistance = originalFogEnd;
		}
	}
}
