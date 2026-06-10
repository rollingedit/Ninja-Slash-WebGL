using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinStorePanel : MonoBehaviour
{
	private void OnGetObjectFromPool()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnOpenCoinStoreEvent());
	}
}
