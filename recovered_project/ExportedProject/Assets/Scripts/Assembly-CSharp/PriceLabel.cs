using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PriceLabel : MonoBehaviour
{
	public ShopItem.Type itemType;

	public UILabel uiLabel;

	private void SetLabel()
	{
		if (MonoSingleton<ShopManager>.instance.GetItemPrice(itemType) != 2147483647)
		{
			uiLabel.text = MonoSingleton<ShopManager>.instance.GetItemPrice(itemType).ToString();
		}
		else
		{
			uiLabel.text = "Full";
		}
	}

	private void OnGetObjectFromPool()
	{
		SetLabel();
	}

	private void OnChangePrice()
	{
		SetLabel();
	}
}
