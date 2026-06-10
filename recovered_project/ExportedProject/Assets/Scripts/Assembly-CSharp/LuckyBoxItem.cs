using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class LuckyBoxItem
{
	[SerializeField]
	public Reward reward;

	[SerializeField]
	public float probability;
}
