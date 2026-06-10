public class ScoreLabel : BaseLabel
{
	public UILabel uiLabel;

	private int score;

	public override void Initialize()
	{
		score = 0;
	}

	public override void UpdateLabel()
	{
		uiLabel.text = score.ToString();
	}

	private void OnGetScore(int scoreIn)
	{
		score = scoreIn;
		UpdateLabel();
	}
}
