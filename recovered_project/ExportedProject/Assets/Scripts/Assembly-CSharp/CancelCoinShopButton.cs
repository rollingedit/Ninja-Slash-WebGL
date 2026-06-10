using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CancelCoinShopButton : MonoBehaviour
{
	public GameObject popUpWindow;

	private void OnClick()
	{
		popUpWindow.SetActive(false);
	}
}
