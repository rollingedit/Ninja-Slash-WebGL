using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ItemNumberLabelOnShopWindow : MonoBehaviour
{
	public UILabel uiLabel;

	public ShopItem.Type itemType;

	private void SetLabel()
	{
		if (itemType == ShopItem.Type.multiplierBooster)
		{
			uiLabel.text = string.Concat((object)"You have: ", (object)MonoSingleton<UserData>.instance.MultiplierBoosterCount);
		}
		else if (itemType == ShopItem.Type.shield)
		{
			uiLabel.text = string.Concat((object)"You have: ", (object)MonoSingleton<UserData>.instance.ShieldCount);
		}
	}

	private void OnGetObjectFromPool()
	{
		SetLabel();
	}

	private void OnItemNumberChanged()
	{
		SetLabel();
	}
}
