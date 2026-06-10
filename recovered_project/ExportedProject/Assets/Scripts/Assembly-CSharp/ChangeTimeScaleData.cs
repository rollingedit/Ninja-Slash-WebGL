public class ChangeTimeScaleData
{
	private float timeScale;

	private float duration;

	public float TimeScale
	{
		get
		{
			return timeScale;
		}
	}

	public float Duration
	{
		get
		{
			return duration;
		}
	}

	public ChangeTimeScaleData(float timeScaleIn, float durationIn)
	{
		timeScale = timeScaleIn;
		duration = durationIn;
	}
}
