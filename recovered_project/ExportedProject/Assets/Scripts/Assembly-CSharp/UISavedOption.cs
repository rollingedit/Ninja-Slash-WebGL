using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private UIPopupList mList;

	private UICheckbox mCheck;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(keyName)) ? keyName : ("NGUI State: " + ((Object)this).name);
		}
	}

	private void Awake()
	{
		mList = ((Component)this).GetComponent<UIPopupList>();
		mCheck = ((Component)this).GetComponent<UICheckbox>();
		if ((Object)(object)mList != (Object)null)
		{
			UIPopupList uIPopupList = mList;
			uIPopupList.onSelectionChange = (UIPopupList.OnSelectionChange)global::System.Delegate.Combine((global::System.Delegate)uIPopupList.onSelectionChange, (global::System.Delegate)new UIPopupList.OnSelectionChange(SaveSelection));
		}
		if ((Object)(object)mCheck != (Object)null)
		{
			UICheckbox uICheckbox = mCheck;
			uICheckbox.onStateChange = (UICheckbox.OnStateChange)global::System.Delegate.Combine((global::System.Delegate)uICheckbox.onStateChange, (global::System.Delegate)new UICheckbox.OnStateChange(SaveState));
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)mCheck != (Object)null)
		{
			UICheckbox uICheckbox = mCheck;
			uICheckbox.onStateChange = (UICheckbox.OnStateChange)global::System.Delegate.Remove((global::System.Delegate)uICheckbox.onStateChange, (global::System.Delegate)new UICheckbox.OnStateChange(SaveState));
		}
		if ((Object)(object)mList != (Object)null)
		{
			UIPopupList uIPopupList = mList;
			uIPopupList.onSelectionChange = (UIPopupList.OnSelectionChange)global::System.Delegate.Remove((global::System.Delegate)uIPopupList.onSelectionChange, (global::System.Delegate)new UIPopupList.OnSelectionChange(SaveSelection));
		}
	}

	private void OnEnable()
	{
		string text = PlayerPrefs.GetString(key);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if ((Object)(object)mList != (Object)null)
		{
			mList.selection = text;
			return;
		}
		if ((Object)(object)mCheck != (Object)null)
		{
			mCheck.isChecked = text == "true";
			return;
		}
		UICheckbox[] componentsInChildren = ((Component)this).GetComponentsInChildren<UICheckbox>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox uICheckbox = componentsInChildren[i];
			uICheckbox.isChecked = ((Object)uICheckbox).name == text;
		}
	}

	private void OnDisable()
	{
		if (!((Object)(object)mCheck == (Object)null) || !((Object)(object)mList == (Object)null))
		{
			return;
		}
		UICheckbox[] componentsInChildren = ((Component)this).GetComponentsInChildren<UICheckbox>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox uICheckbox = componentsInChildren[i];
			if (uICheckbox.isChecked)
			{
				SaveSelection(((Object)uICheckbox).name);
				break;
			}
		}
	}

	private void SaveSelection(string selection)
	{
		PlayerPrefs.SetString(key, selection);
	}

	private void SaveState(bool state)
	{
		PlayerPrefs.SetString(key, (!state) ? "false" : "true");
	}
}
