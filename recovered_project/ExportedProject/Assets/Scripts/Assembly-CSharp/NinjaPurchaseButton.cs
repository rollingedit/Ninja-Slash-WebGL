using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class NinjaPurchaseButton : MonoBehaviour
{
	public UILabel uiLabel;

	public AudioClip clip;

	private void OnSetNinjaPanelCell(Ninja ninja)
	{
		uiLabel.text = ninja.unlockCost.ToString();
	}

	private void OnClick()
	{
		((Component)this).SendMessageUpwards("OnClickNinjaPurchaseButton");
	}

	private void OnPurchaseSuccess()
	{
		MonoSingleton<SoundManager>.instance.PlaySound(clip);
	}
}
