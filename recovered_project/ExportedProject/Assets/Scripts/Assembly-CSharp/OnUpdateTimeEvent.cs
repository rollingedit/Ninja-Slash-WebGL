public class OnUpdateTimeEvent : BaseEvent
{
	protected UpdateTimeData data;

	public OnUpdateTimeEvent(UpdateTimeData dataIn)
	{
		data = dataIn;
	}

	public override object GetData()
	{
		return data;
	}
}
