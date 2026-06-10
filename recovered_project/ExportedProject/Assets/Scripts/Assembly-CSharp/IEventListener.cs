public interface IEventListener
{
	bool isEnabled();

	bool HandleEvent(IEvent evt);
}
