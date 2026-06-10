using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OpenCoinShopButton : MonoBehaviour
{
	public GameObject popupWindow;

	public GameObject coinShopWindow;

	private void OnClick()
	{
		popupWindow.SetActive(false);
		coinShopWindow.SetActive(true);
	}
}
