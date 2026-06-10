public class OnAlmostFinishBuffEvent : BaseEvent
{
	private ScrollElement element;

	public OnAlmostFinishBuffEvent(ScrollElement elementIn)
	{
		element = elementIn;
	}

	public override object GetData()
	{
		return element;
	}
}
