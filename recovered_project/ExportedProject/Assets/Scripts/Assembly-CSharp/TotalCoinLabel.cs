public class TotalCoinLabel : BaseLabel
{
	public UILabel uiLabel;

	public override void Initialize()
	{
	}

	public override void UpdateLabel()
	{
		uiLabel.text = MonoSingleton<UserData>.instance.Coin.ToString();
	}

	private void OnGetObjectFromPool()
	{
		UpdateLabel();
	}

	private void OnTotalCoinChanged()
	{
		UpdateLabel();
	}

	private void OnGameOver()
	{
		UpdateLabel();
	}
}
