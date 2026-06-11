using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OptionCheckBox : MonoBehaviour
{
	public OptionCheckBoxType type;

	public UICheckbox uiCheckbox;

	private void OnGetObjectFromPool()
	{
		if (type == OptionCheckBoxType.bgm)
		{
			uiCheckbox.isChecked = MonoSingleton<UserData>.instance.IsBGMOn;
		}
		else if (type == OptionCheckBoxType.effect)
		{
			uiCheckbox.isChecked = MonoSingleton<UserData>.instance.IsEffectSoundOn;
		}
		else if (type == OptionCheckBoxType.tutorial)
		{
			uiCheckbox.isChecked = !MonoSingleton<UserData>.instance.IsTutorialDone;
		}
	}

	private void OnActivate(bool isChecked)
	{
		ApplyState(isChecked);
	}

	private void ApplyState(bool isChecked)
	{
		((Component)this).SendMessageUpwards(isChecked ? "OnChecked" : "OnUnchecked", (object)type);
	}
}
