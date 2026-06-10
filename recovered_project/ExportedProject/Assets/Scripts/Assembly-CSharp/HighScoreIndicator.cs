using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class HighScoreIndicator : MonoBehaviour
{
	public UILabel label;

	private void OnGameOver(GameOverResult resultIn)
	{
		if (resultIn.IsHighScore)
		{
			((Behaviour)label).enabled = true;
		}
		else
		{
			((Behaviour)label).enabled = false;
		}
	}
}
