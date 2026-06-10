using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FacebookRecommendButton : MonoBehaviour
{
	public UIButton uiButton;

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnFacebookRecommendEvent());
	}

	private void OnGameStart()
	{
		uiButton.isEnabled = false;
	}

	private void OnGameOver()
	{
		uiButton.isEnabled = true;
	}
}
