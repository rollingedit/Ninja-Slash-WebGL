public class CoinLabelOnHUD : BaseLabel
{
	public UILabel uiLabel;

	private int coin;

	public override void UpdateLabel()
	{
		uiLabel.text = coin.ToString();
	}

	public override void Initialize()
	{
		coin = 0;
	}

	private void OnGetCoin(int coinIn)
	{
		coin += coinIn;
		UpdateLabel();
	}
}
