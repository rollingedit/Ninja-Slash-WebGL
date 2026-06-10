public class HighScoreLabel : BaseLabel
{
	public UILabel uiLabel;

	private int highScore;

	public override void Initialize()
	{
	}

	public override void UpdateLabel()
	{
		uiLabel.text = highScore.ToString();
	}

	private void OnGameOver()
	{
		highScore = MonoSingleton<UserData>.instance.HighScore;
		UpdateLabel();
	}
}
