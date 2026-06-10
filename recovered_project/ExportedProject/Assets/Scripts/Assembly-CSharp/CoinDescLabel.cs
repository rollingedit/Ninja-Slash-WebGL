using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinDescLabel : MonoBehaviour
{
	public UILabel label;

	private void SetDesc(string descIn)
	{
		label.text = descIn;
	}
}
