using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	public InvEquipment equipment;

	public InvBaseItem.Slot slot;

	protected override InvGameItem observedItem
	{
		get
		{
			return (!((Object)(object)equipment != (Object)null)) ? null : equipment.GetItem(slot);
		}
	}

	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!((Object)(object)equipment != (Object)null)) ? item : equipment.Replace(slot, item);
	}
}
