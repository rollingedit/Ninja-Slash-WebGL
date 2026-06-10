using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CoinLabelOnGameOverWindow : MonoBehaviour
{
	public UILabel label;

	private void OnGameOver(GameOverResult resultIn)
	{
		label.text = resultIn.Coin.ToString();
	}
}
