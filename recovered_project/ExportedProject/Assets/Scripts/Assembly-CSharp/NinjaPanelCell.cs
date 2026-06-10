using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class NinjaPanelCell : MonoBehaviour
{
	public string ninjaName;

	public GameObject selectButton;

	public GameObject tokenLabel;

	public GameObject purchaseButton;

	public GameObject focusedMark;

	public GameObject ninjaRoom;

	private Ninja myNinja;

	private void Awake()
	{
		myNinja = MonoSingleton<NinjaInfo>.instance.GetNinjaByName(ninjaName);
	}

	private void OnGetObjectFromPool()
	{
		if (myNinja == null)
		{
			myNinja = MonoSingleton<NinjaInfo>.instance.GetNinjaByName(ninjaName);
		}
		if (myNinja == null)
		{
			return;
		}
		if (myNinja.IsNinjaUnlocked())
		{
			SetActiveIfPresent(selectButton, true);
			SetActiveIfPresent(purchaseButton, false);
			SetActiveIfPresent(tokenLabel, false);
		}
		else
		{
			SetActiveIfPresent(selectButton, false);
			if (myNinja.howToUnlock == Ninja.HowToUnlock.coin)
			{
				SetActiveIfPresent(purchaseButton, true);
				SetActiveIfPresent(tokenLabel, false);
			}
			else
			{
				SetActiveIfPresent(tokenLabel, true);
				SetActiveIfPresent(purchaseButton, false);
			}
		}
		SendMessageIfPresent(selectButton, "OnSetNinjaPanelCell", myNinja);
		SendMessageIfPresent(tokenLabel, "OnSetNinjaPanelCell", myNinja);
		SendMessageIfPresent(purchaseButton, "OnSetNinjaPanelCell", myNinja);
	}

	private void OnWindowMoved(bool isFocused)
	{
		SendMessageIfPresent(focusedMark, "OnWindowMoved", isFocused);
		if ((Object)(object)ninjaRoom != (Object)null)
		{
			ninjaRoom.BroadcastMessage("OnWindowMoved", (object)isFocused, (SendMessageOptions)1);
		}
	}

	private void OnModelChanged()
	{
		Ninja currentSelectedNinja = MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja();
		if (myNinja == null || currentSelectedNinja == null)
		{
			return;
		}
		SendMessageIfPresent(selectButton, "OnModelChanged", myNinja.ninjaName == currentSelectedNinja.ninjaName);
	}

	private void OnClickNinjaPurchaseButton()
	{
		if (myNinja == null)
		{
			return;
		}
		if (MonoSingleton<UserData>.instance.Coin >= 0)
		{
			MonoSingleton<UserData>.instance.SpendCoin(myNinja.unlockCost);
			myNinja.isUnlocked = true;
			SendMessageIfPresent(purchaseButton, "OnPurchaseSuccess", null);
			OnGetObjectFromPool();
		}
	}

	private void SetActiveIfPresent(GameObject go, bool active)
	{
		if ((Object)(object)go != (Object)null)
		{
			go.SetActive(active);
		}
	}

	private void SendMessageIfPresent(GameObject go, string method, object value)
	{
		if ((Object)(object)go != (Object)null)
		{
			go.SendMessage(method, value, (SendMessageOptions)1);
		}
	}
}
