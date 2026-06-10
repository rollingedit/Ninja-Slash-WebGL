public abstract class BaseEvent : IEvent
{
	public virtual string GetName()
	{
		return base.GetType().ToString();
	}

	public virtual object GetData()
	{
		return null;
	}
}
