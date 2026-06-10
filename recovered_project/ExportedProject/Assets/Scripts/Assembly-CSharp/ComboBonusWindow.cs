public class ComboBonusWindow : MessageWindow
{
	public UILabel comboLabel;

	public UILabel rewardCountLabel;

	public UISprite rewardSprite;

	private void OnComboBonus(ComboBonus bonusIn)
	{
		comboLabel.text = string.Concat((object)bonusIn.comboLimit, (object)" Combo Bonus");
		rewardCountLabel.text = string.Concat((object)"x", (object)bonusIn.reward.quantity);
		rewardSprite.spriteName = bonusIn.reward.iconSpriteName;
	}
}
