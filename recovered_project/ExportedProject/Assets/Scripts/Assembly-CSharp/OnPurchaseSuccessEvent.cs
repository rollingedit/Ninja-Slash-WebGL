public class OnPurchaseSuccessEvent : BaseEvent
{
	private ShopItem.Type itemType;

	public OnPurchaseSuccessEvent(ShopItem.Type itemTypeIn)
	{
		itemType = itemTypeIn;
	}

	public override object GetData()
	{
		return itemType;
	}
}
