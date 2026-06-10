public class OnPurchaseEvent : BaseEvent
{
	private ShopItem.Type itemType;

	public OnPurchaseEvent(ShopItem.Type itemTypeIn)
	{
		itemType = itemTypeIn;
	}

	public override object GetData()
	{
		return itemType;
	}
}
