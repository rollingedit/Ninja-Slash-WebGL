using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class HighComboLabelOnGameOverWindow : MonoBehaviour
{
	public UILabel uiLabel;

	private void OnGameOver(GameOverResult resultIn)
	{
		uiLabel.text = resultIn.Combo.ToString();
	}
}
