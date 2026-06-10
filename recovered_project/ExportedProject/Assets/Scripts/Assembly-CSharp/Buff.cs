using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class Buff
{
	[SerializeField]
	public GameObject buffPrefab;

	[SerializeField]
	public float defaultTime;

	[SerializeField]
	public float timePerUpgrade;

	[SerializeField]
	public GameObject effectPrefab;

	[SerializeField]
	public GameObject effectPrefabForLowPerformance;

	[SerializeField]
	public GameObject letterPrefab;
}
