using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class HomeButton : MonoBehaviour
{
	public GameObject targetWindow;

	private void OnClick()
	{
		targetWindow.SendMessage("OnHomeButtonClicked");
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnHomeEvent());
	}
}
