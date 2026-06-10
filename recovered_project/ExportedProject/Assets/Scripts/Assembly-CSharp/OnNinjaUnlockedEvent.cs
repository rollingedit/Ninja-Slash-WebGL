public class OnNinjaUnlockedEvent : BaseEvent
{
	private Ninja ninja;

	public OnNinjaUnlockedEvent(Ninja ninjaIn)
	{
		ninja = ninjaIn;
	}

	public override object GetData()
	{
		return ninja;
	}
}
