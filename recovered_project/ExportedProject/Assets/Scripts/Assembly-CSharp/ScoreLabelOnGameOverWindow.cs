using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ScoreLabelOnGameOverWindow : MonoBehaviour
{
	public UILabel label;

	private void OnGameOver(GameOverResult resultIn)
	{
		label.text = resultIn.Score.ToString();
	}
}
