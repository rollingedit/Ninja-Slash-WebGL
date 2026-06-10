using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CameraEffect : MonoBehaviour
{
	public GameObject startSmokeEffect;

	public GameObject shieldSmokeEffect;

	public GameObject effectAnchor;

	private void OnGameStart()
	{
		PoolManager.SpawnAndAttachToParent(startSmokeEffect, effectAnchor);
	}

	private void OnShieldUsed()
	{
		PoolManager.SpawnAndAttachToParent(shieldSmokeEffect, effectAnchor);
	}
}
