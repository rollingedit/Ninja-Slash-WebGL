using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShopPanelButton : MonoBehaviour
{
	public ShopPanelType panelType;

	public GameObject selectedMark;

	private void OnClick()
	{
		((Component)this).SendMessageUpwards("OnClickButton", (object)panelType);
	}

	private void OnSetShopPanelButton(ShopPanelType panelTypeIn)
	{
		if (panelType == panelTypeIn)
		{
			selectedMark.SetActive(true);
		}
		else
		{
			selectedMark.SetActive(false);
		}
	}
}
