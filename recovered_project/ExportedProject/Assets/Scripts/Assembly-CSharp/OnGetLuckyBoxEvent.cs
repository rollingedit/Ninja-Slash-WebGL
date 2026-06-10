public class OnGetLuckyBoxEvent : BaseEvent
{
	private Reward item;

	public OnGetLuckyBoxEvent(Reward itemIn)
	{
		item = itemIn;
	}

	public override object GetData()
	{
		return item;
	}
}
