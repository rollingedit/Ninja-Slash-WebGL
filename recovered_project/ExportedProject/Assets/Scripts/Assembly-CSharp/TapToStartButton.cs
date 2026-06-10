using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TapToStartButton : MonoBehaviour
{
	public string buttonTextWeb;

	public string buttonTextMobile;

	public UIButton button;

	public UILabel[] labels;

	public float waitTime;

	private void Awake()
	{
		string text = ((!Utility.IsMobile()) ? buttonTextWeb : buttonTextMobile);
		UILabel[] array = labels;
		foreach (UILabel uILabel in array)
		{
			uILabel.text = text;
		}
	}

	private void Update()
	{
		if ((Input.GetKeyDown((KeyCode)114) || Input.GetKeyDown(KeyCode.Space)) && button.isEnabled && !OpenWindowButton.IsWindowOpen)
		{
			OnClick();
		}
	}

	private void OnEnable()
	{
		button.isEnabled = true;
	}

	private void OnOpenWindow()
	{
		((Behaviour)this).enabled = false;
	}

	private void OnCloseWindow()
	{
		((Behaviour)this).enabled = true;
	}

	private void OnClick()
	{
		if (!button.isEnabled || OpenWindowButton.IsWindowOpen)
		{
			return;
		}
		((Component)((Component)this).transform.parent).BroadcastMessage("OnTapToStartButtonClicked");
		button.isEnabled = false;
		((MonoBehaviour)this).StartCoroutine("WaitAndStart");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndStart()
	{
		yield return (object)new WaitForSeconds(waitTime);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGameStartEvent());
	}
}
