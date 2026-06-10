using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class StatPanel : MonoBehaviour
{
	public UILabel highScoreLabel;

	public UILabel maxComboLabel;

	private void OnGetObjectFromPool()
	{
		highScoreLabel.text = MonoSingleton<UserData>.instance.HighScore.ToString();
		maxComboLabel.text = MonoSingleton<UserData>.instance.HighCombo.ToString();
	}
}
