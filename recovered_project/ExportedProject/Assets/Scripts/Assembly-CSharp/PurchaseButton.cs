using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PurchaseButton : MonoBehaviour
{
	public ShopItem.Type itemType;

	public AudioClip clip;

	public UIButton button;

	private void OnEnable()
	{
		button.isEnabled = true;
	}

	private void OnGetObjectFromPool()
	{
		DisableButtonOnFull();
	}

	private void OnClick()
	{
		if (button.isEnabled)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnPurchaseEvent(itemType));
			if (itemType == ShopItem.Type.luckyBox)
			{
				((MonoBehaviour)this).StartCoroutine("DisableButton", (object)2.5f);
			}
			else if (itemType == ShopItem.Type.skipMission)
			{
				((MonoBehaviour)this).StartCoroutine("DisableButton", (object)5f);
			}
		}
	}

	private void OnPurchaseSuccess(ShopItem.Type itemTypeIn)
	{
		if (itemType == itemTypeIn)
		{
			MonoSingleton<SoundManager>.instance.PlaySound(clip);
		}
	}

	private void OnChangePrice()
	{
		DisableButtonOnFull();
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DisableButton(float waitTime)
	{
		button.isEnabled = false;
		yield return (object)new WaitForSeconds(waitTime);
		button.isEnabled = true;
	}

	private void DisableButtonOnFull()
	{
		if (MonoSingleton<ShopManager>.instance.GetItemPrice(itemType) == 2147483647)
		{
			button.isEnabled = false;
		}
	}
}
