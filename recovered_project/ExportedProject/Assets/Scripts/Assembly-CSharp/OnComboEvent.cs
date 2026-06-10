public class OnComboEvent : BaseEvent
{
	private int combo;

	public OnComboEvent(int comboIn)
	{
		combo = comboIn;
	}

	public override object GetData()
	{
		return combo;
	}
}
