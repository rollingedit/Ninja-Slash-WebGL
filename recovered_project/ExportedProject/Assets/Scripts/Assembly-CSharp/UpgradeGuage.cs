using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class UpgradeGuage : MonoBehaviour
{
	public UIFilledSprite uiFilledSprite;

	public ScrollElement element;

	private void OnGetObjectFromPool()
	{
		SetAmount();
	}

	private void OnPurchaseSuccess()
	{
		SetAmount();
	}

	private void SetAmount()
	{
		uiFilledSprite.fillAmount = (float)MonoSingleton<UserData>.instance.GetUpgradeLevel(element) / 5f;
	}
}
