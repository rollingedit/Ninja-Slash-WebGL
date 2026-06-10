public class OnInAppPurchaseEvent : BaseEvent
{
	private string productIdentifier;

	public OnInAppPurchaseEvent(string productIdentifierIn)
	{
		productIdentifier = productIdentifierIn;
	}

	public override object GetData()
	{
		return productIdentifier;
	}
}
