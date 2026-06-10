public class OnResumeCountEvent : BaseEvent
{
	private float leftTime;

	public OnResumeCountEvent(float leftTimeIn)
	{
		leftTime = leftTimeIn;
	}

	public override object GetData()
	{
		return leftTime;
	}
}
