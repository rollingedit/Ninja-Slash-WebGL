public class OnShurikenKilledZombieEvent : BaseEvent
{
	private ZombieType zName;

	public OnShurikenKilledZombieEvent(ZombieType zNameIn)
	{
		zName = zNameIn;
	}

	public override object GetData()
	{
		return zName;
	}
}
