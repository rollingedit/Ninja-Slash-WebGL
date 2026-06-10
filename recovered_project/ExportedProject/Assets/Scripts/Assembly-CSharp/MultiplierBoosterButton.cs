using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MultiplierBoosterButton : MonoBehaviour
{
	public TweenPosition tweenPosition;

	public UILabel uiLabel;

	private bool isPressed;

	private void Initialize()
	{
		isPressed = false;
		if (MonoSingleton<UserData>.instance.MultiplierBoosterCount > 0 && MonoSingleton<UserData>.instance.IsTutorialDone)
		{
			tweenPosition.Play(true);
			((MonoBehaviour)this).StartCoroutine("WaitAndHide");
			uiLabel.text = MonoSingleton<UserData>.instance.MultiplierBoosterCount.ToString();
		}
	}

	private void OnGameStart()
	{
		Initialize();
	}

	private void OnHome()
	{
		((MonoBehaviour)this).StopCoroutine("WaitAndHide");
		tweenPosition.Play(true);
		((Behaviour)tweenPosition).enabled = false;
		tweenPosition.Reset();
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndHide()
	{
		yield return (object)new WaitForSeconds(3f);
		tweenPosition.Play(false);
	}

	private void OnClick()
	{
		if (!isPressed)
		{
			((MonoBehaviour)this).StopCoroutine("WaitAndHide");
			tweenPosition.Play(false);
			isPressed = true;
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnUseMultiplierBoosterEvent());
			uiLabel.text = MonoSingleton<UserData>.instance.MultiplierBoosterCount.ToString();
		}
	}
}
