public class OnChangeTimeScaleEvent : BaseEvent
{
	private float timeScale;

	private float duration;

	public OnChangeTimeScaleEvent(float timeScaleIn, float durationIn)
	{
		timeScale = timeScaleIn;
		duration = durationIn;
	}

	public override object GetData()
	{
		return new ChangeTimeScaleData(timeScale, duration);
	}
}
