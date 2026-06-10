public class OnBuyLuckyBoxEvent : BaseEvent
{
	private Reward item;

	public OnBuyLuckyBoxEvent(Reward itemIn)
	{
		item = itemIn;
	}

	public override object GetData()
	{
		return item;
	}
}
