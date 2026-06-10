using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public abstract class BaseLabel : MonoBehaviour
{
	private void OnEnable()
	{
		Initialize();
		UpdateLabel();
	}

	private void OnGameStart()
	{
		Initialize();
		UpdateLabel();
	}

	public abstract void Initialize();

	public abstract void UpdateLabel();
}
