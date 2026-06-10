public class OnFinishBuffEvent : BaseEvent
{
	private ScrollElement element;

	public OnFinishBuffEvent(ScrollElement elementIn)
	{
		element = elementIn;
	}

	public override object GetData()
	{
		return element;
	}
}
