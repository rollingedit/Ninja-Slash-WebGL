using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OptionWindow : MonoBehaviour
{
	public TweenPosition tweenPosition;

	public GameObject[] tutorial;

	public GameObject tutorialDisableText;

	private bool isClosing;

	private void OnGetObjectFromPool()
	{
		isClosing = false;
		tweenPosition.Play(true);
	}

	private void OnBackButtonClicked()
	{
		if (isClosing)
		{
			return;
		}
		isClosing = true;
		tweenPosition.Play(false);
		((Component)((Component)this).transform.root).BroadcastMessage("OnCloseWindow", (SendMessageOptions)1);
		NGUIUtility.DestroyWhenTweenFinished(((Component)this).gameObject, tweenPosition);
	}

	private void OnCheckboxClicked(OptionCheckBoxType typeIn, bool isChecked)
	{
		switch (typeIn)
		{
		case OptionCheckBoxType.bgm:
			MonoSingleton<UserData>.instance.IsBGMOn = isChecked;
			MonoSingleton<SoundManager>.instance.OnChangeBGMState();
			break;
		case OptionCheckBoxType.effect:
			MonoSingleton<UserData>.instance.IsEffectSoundOn = isChecked;
			MonoSingleton<SoundManager>.instance.OnChangeEffectSoundState();
			break;
		case OptionCheckBoxType.tutorial:
			MonoSingleton<UserData>.instance.IsTutorialDone = !isChecked;
			MonoSingleton<MapManager>.instance.ResetMap();
			break;
		}
	}

	private void OnChecked(OptionCheckBoxType typeIn)
	{
		OnCheckboxClicked(typeIn, true);
	}

	private void OnUnchecked(OptionCheckBoxType typeIn)
	{
		OnCheckboxClicked(typeIn, false);
	}

	private void OnOptionWindowOpened(bool isGameStarted)
	{
		if (isGameStarted)
		{
			GameObject[] array = tutorial;
			foreach (GameObject val in array)
			{
				val.SetActive(false);
			}
			tutorialDisableText.SetActive(true);
		}
		else
		{
			GameObject[] array2 = tutorial;
			foreach (GameObject val2 in array2)
			{
				val2.SetActive(true);
			}
			tutorialDisableText.SetActive(false);
		}
	}
}
