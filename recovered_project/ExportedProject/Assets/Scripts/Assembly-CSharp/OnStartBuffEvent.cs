public class OnStartBuffEvent : BaseEvent
{
	private ScrollElement element;

	public OnStartBuffEvent(ScrollElement elementIn)
	{
		element = elementIn;
	}

	public override object GetData()
	{
		return element;
	}
}
