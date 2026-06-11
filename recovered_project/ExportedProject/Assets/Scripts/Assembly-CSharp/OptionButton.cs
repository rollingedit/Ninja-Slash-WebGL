using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OptionButton : MonoBehaviour
{
	public UIButton button;

	private void OnHome()
	{
		button.isEnabled = true;
	}

	private void OnTapToStartButtonClicked()
	{
		button.isEnabled = false;
	}

	private void OnClick()
	{
		if (OpenWindowButton.IsWindowOpen)
		{
			return;
		}
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnEnterOptionEvent());
		button.isEnabled = false;
	}

	private void OnOpenWindow()
	{
		button.isEnabled = false;
	}

	private void OnCloseWindow()
	{
		button.isEnabled = true;
	}
}
