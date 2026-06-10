using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PauseButton : MonoBehaviour
{
	public UIButton button;

	private void Update()
	{
		if (Input.GetKeyDown((KeyCode)112) && button.isEnabled)
		{
			OnClick();
		}
	}

	private void OnEnable()
	{
		button.isEnabled = true;
	}

	private void OnGameStart()
	{
		button.isEnabled = true;
	}

	private void OnRestart()
	{
		button.isEnabled = true;
	}

	private void OnDead()
	{
		button.isEnabled = false;
	}

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnPauseEvent());
		button.isEnabled = false;
	}

	private void OnResume()
	{
		button.isEnabled = true;
	}
}
