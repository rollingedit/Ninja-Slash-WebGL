using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinTitleLabel : MonoBehaviour
{
	public UILabel label;

	private void SetTitle(string titleIn)
	{
		label.text = titleIn;
	}
}
