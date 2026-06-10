using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Component")]
public class UICheckboxControlledComponent : MonoBehaviour
{
	public MonoBehaviour target;

	public bool inverse;

	private bool mUsingDelegates;

	private void Start()
	{
		UICheckbox component = ((Component)this).GetComponent<UICheckbox>();
		if ((Object)(object)component != (Object)null)
		{
			mUsingDelegates = true;
			component.onStateChange = (UICheckbox.OnStateChange)global::System.Delegate.Combine((global::System.Delegate)component.onStateChange, (global::System.Delegate)new UICheckbox.OnStateChange(OnActivateDelegate));
		}
	}

	private void OnActivateDelegate(bool isActive)
	{
		if (((Behaviour)this).enabled && (Object)(object)target != (Object)null)
		{
			((Behaviour)target).enabled = ((!inverse) ? isActive : (!isActive));
		}
	}

	private void OnActivate(bool isActive)
	{
		if (!mUsingDelegates)
		{
			OnActivateDelegate(isActive);
		}
	}
}
