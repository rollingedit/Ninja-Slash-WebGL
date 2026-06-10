using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FacebookConnectButton : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnFacebookConnectEvent());
	}
}
