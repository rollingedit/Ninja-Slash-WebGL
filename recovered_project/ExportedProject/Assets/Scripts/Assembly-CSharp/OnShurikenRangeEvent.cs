public class OnShurikenRangeEvent : BaseEvent
{
	private ShurikenRangeData data;

	public OnShurikenRangeEvent(ShurikenRangeData dataIn)
	{
		data = dataIn;
	}

	public override object GetData()
	{
		return data;
	}
}
