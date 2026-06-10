public class NinjaUnlockWindow : MessageWindow
{
	public UILabel uiLabel;

	private void OnNinjaUnlocked(Ninja ninja)
	{
		uiLabel.text = ninja.ninjaName;
	}
}
