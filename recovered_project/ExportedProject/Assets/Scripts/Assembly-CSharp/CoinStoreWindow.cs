using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinStoreWindow : MonoBehaviour
{
	public GameObject coinStorePanelPrefab;

	private GameObject coinStorePanel;

	private void OnEnable()
	{
		if ((Object)(object)coinStorePanel != (Object)null)
		{
			PoolManager.Despawn(coinStorePanel);
		}
		coinStorePanel = NGUIUtility.AddChild(((Component)this).gameObject, coinStorePanelPrefab);
	}

	private void OnDisable()
	{
	}
}
