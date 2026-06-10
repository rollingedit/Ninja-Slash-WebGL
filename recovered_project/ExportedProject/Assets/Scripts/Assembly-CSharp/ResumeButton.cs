using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ResumeButton : MonoBehaviour
{
	public UIButton uiButton;

	private void Update()
	{
		if (Input.GetKeyDown((KeyCode)112) && uiButton.isEnabled)
		{
			OnClick();
		}
	}

	private void OnGetObjectFromPool()
	{
		uiButton.isEnabled = true;
	}

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnDoResumeEvent());
		uiButton.isEnabled = false;
	}
}
