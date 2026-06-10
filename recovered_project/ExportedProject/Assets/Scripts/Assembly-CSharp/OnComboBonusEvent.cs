public class OnComboBonusEvent : BaseEvent
{
	private ComboBonus bonus;

	public OnComboBonusEvent(ComboBonus bonusIn)
	{
		bonus = bonusIn;
	}

	public override object GetData()
	{
		return bonus;
	}
}
