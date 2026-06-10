public class OnGetScoreEvent : BaseEvent
{
	private int score;

	public OnGetScoreEvent(int scoreIn)
	{
		score = scoreIn;
	}

	public override object GetData()
	{
		return score;
	}
}
