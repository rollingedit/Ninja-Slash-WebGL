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
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnEnterOptionEvent());
		((MonoBehaviour)this).StartCoroutine("DisableButton");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DisableButton()
	{
		button.isEnabled = false;
		yield return (object)new WaitForSeconds(0.5f);
		button.isEnabled = true;
	}
}
