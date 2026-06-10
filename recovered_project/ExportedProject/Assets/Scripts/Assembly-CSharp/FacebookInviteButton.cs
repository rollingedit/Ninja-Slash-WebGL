using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FacebookInviteButton : MonoBehaviour
{
	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnFacebookInviteEvent());
	}
}
