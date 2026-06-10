using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class InAppPurchase : MonoBehaviour
{
	public string[] productIdentifiers;

	private void RequestProductData()
	{
		Debug.Log((object)"OnRequestProductData");
	}

	private void BuyProduct(string productIdentifier)
	{
		Debug.Log((object)("OnInAppPurchase: " + productIdentifier));
	}

	private void OnInAppPurchase(string productIdentifier)
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnStartDownloadingEvent());
		BuyProduct(productIdentifier);
	}

	private void OnOpenCoinStore()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnStartDownloadingEvent());
		RequestProductData();
	}
}
