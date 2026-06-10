public class GameOverResult
{
	private int score;

	private int coin;

	private int combo;

	private int kills;

	private bool isHighScore;

	public bool IsHighScore
	{
		get
		{
			return isHighScore;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
	}

	public int Coin
	{
		get
		{
			return coin;
		}
	}

	public int Combo
	{
		get
		{
			return combo;
		}
	}

	public int Kill
	{
		get
		{
			return kills;
		}
	}

	public GameOverResult(int scoreIn, int coinIn, int comboIn, int killsIn, bool isHighScoreIn)
	{
		score = scoreIn;
		coin = coinIn;
		combo = comboIn;
		isHighScore = isHighScoreIn;
		kills = killsIn;
	}
}
