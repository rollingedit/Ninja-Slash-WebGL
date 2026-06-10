public class OnGameOverEvent : BaseEvent
{
	private GameOverResult gameOverResult;

	public OnGameOverEvent(GameOverResult resultIn)
	{
		gameOverResult = resultIn;
	}

	public override object GetData()
	{
		return gameOverResult;
	}
}
