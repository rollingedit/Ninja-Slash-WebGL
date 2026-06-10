public class HighComboLabel : BaseLabel
{
	public UILabel uiLabel;

	private int highCombo;

	public override void Initialize()
	{
	}

	public override void UpdateLabel()
	{
		uiLabel.text = highCombo.ToString();
	}

	private void OnGameOver()
	{
		highCombo = MonoSingleton<UserData>.instance.HighCombo;
		UpdateLabel();
	}
}
