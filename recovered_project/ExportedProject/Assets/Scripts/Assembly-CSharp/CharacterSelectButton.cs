using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterSelectButton : MonoBehaviour
{
	public UILabel uiLabel;

	public UIButton button;

	public AudioClip clip;

	private void OnSetNinjaPanelCell(Ninja ninja)
	{
		if (ninja.ninjaName == MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja().ninjaName)
		{
			uiLabel.text = "selected";
			button.isEnabled = false;
		}
	}

	private void OnModelChanged(bool selected)
	{
		if (selected)
		{
			uiLabel.text = "selected";
			button.isEnabled = false;
			MonoSingleton<SoundManager>.instance.PlaySound(clip);
		}
		else
		{
			uiLabel.text = "select";
			button.isEnabled = true;
		}
	}

	private void OnClick()
	{
		((Component)this).SendMessageUpwards("OnClickSelectButton");
	}
}
