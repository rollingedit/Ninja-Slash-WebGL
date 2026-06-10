public class OnAttachTextureEvent : BaseEvent
{
	private AttachTextureData data;

	public OnAttachTextureEvent(AttachTextureData dataIn)
	{
		data = dataIn;
	}

	public override object GetData()
	{
		return data;
	}
}
