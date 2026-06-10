public class OnShieldEvent : BaseEvent
{
	public ShieldState state;

	public OnShieldEvent(ShieldState stateIn)
	{
		state = stateIn;
	}

	public override object GetData()
	{
		return state;
	}
}
