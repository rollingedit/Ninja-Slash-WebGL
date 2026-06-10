using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinPriceLabel : MonoBehaviour
{
	public UILabel label;

	private void SetPrice(string priceIn)
	{
		label.text = priceIn;
	}
}
