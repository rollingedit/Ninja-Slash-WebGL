using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class GameCenterButton : MonoBehaviour
{
	private void Awake()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
