using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class InAppPurchaseButton : MonoBehaviour
{
	public string productIdentifier;

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnInAppPurchaseEvent(productIdentifier));
	}
}
