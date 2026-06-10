public class RewardWindow : MessageWindow
{
	public UILabel rewardLabel;

	public UISprite rewardIcon;

	private void OnGetLuckyBox(Reward itemIn)
	{
		rewardLabel.text = " x " + itemIn.quantity;
		rewardIcon.spriteName = itemIn.iconSpriteName;
	}
}
