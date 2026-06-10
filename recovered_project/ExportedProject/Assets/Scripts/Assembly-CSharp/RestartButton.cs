using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RestartButton : MonoBehaviour
{
	public UIButton uiButton;

	private void Start()
	{
		uiButton.isEnabled = false;
	}

	private void Update()
	{
		if ((Input.GetKeyDown((KeyCode)114) || Input.GetKeyDown(KeyCode.Space)) && uiButton.isEnabled)
		{
			OnClick();
		}
	}

	private void OnClick()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnRestartEvent());
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGameStartEvent());
	}

	private void OnGameStart()
	{
		uiButton.isEnabled = false;
	}

	private void OnGameOver(GameOverResult resultIn)
	{
		uiButton.isEnabled = true;
	}

	private void OnHome()
	{
		uiButton.isEnabled = false;
	}

	private void OnOpenWindow()
	{
		((Behaviour)this).enabled = false;
	}

	private void OnCloseWindow()
	{
		((Behaviour)this).enabled = true;
	}
}
