public class OnGetCoinEvent : BaseEvent
{
	private int coin;

	public OnGetCoinEvent(int coinIn)
	{
		coin = coinIn;
	}

	public override object GetData()
	{
		return coin;
	}
}
