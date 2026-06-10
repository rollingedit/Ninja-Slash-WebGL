using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	private void Start()
	{
		mList = ((Component)this).GetComponent<UIPopupList>();
		UpdateList();
		mList.eventReceiver = ((Component)this).gameObject;
		mList.functionName = "OnLanguageSelection";
	}

	private void UpdateList()
	{
		if (!((Object)(object)Localization.instance != (Object)null) || Localization.instance.languages == null)
		{
			return;
		}
		mList.items.Clear();
		int i = 0;
		for (int num = Localization.instance.languages.Length; i < num; i++)
		{
			TextAsset val = Localization.instance.languages[i];
			if ((Object)(object)val != (Object)null)
			{
				mList.items.Add(((Object)val).name);
			}
		}
		mList.selection = Localization.instance.currentLanguage;
	}

	private void OnLanguageSelection(string language)
	{
		if ((Object)(object)Localization.instance != (Object)null)
		{
			Localization.instance.currentLanguage = language;
		}
	}
}
