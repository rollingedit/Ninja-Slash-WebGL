using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinStoreTab : MonoBehaviour
{
	private void Awake()
	{
		if (!Utility.IsMobile())
		{
			((Component)this).gameObject.SetActive(false);
		}
	}
}
