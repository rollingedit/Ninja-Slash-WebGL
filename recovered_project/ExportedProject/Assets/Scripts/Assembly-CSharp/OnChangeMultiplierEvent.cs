public class OnChangeMultiplierEvent : BaseEvent
{
	private MultiplierData data;

	public OnChangeMultiplierEvent(MultiplierData dataIn)
	{
		data = dataIn;
	}

	public override object GetData()
	{
		return data;
	}
}
