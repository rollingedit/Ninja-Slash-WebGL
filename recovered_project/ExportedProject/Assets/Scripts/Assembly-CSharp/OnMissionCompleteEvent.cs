public class OnMissionCompleteEvent : BaseEvent
{
	private Mission mission;

	public OnMissionCompleteEvent(Mission missionIn)
	{
		mission = missionIn;
	}

	public override object GetData()
	{
		return mission;
	}
}
