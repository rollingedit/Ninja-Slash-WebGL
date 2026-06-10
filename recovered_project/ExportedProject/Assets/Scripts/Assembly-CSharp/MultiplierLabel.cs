using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MultiplierLabel : BaseLabel
{
	public UILabel uiLabel;

	public TweenScale tweenScale;

	public Color colorOnMaxMultiplier;

	public Color colorOnMultiplierBooster;

	public Color colorOnNormal;

	private int multiplier;

	private Color currentNormalColor;

	public override void Initialize()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		multiplier = 0;
		currentNormalColor = colorOnNormal;
		uiLabel.color = currentNormalColor;
	}

	public override void UpdateLabel()
	{
		uiLabel.text = string.Concat((object)"x", (object)multiplier);
	}

	private void OnChangeMultiplier(MultiplierData data)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		multiplier = data.curValue;
		UpdateLabel();
		tweenScale.Reset();
		tweenScale.Play(true);
		if (data.curValue == data.upperLimit)
		{
			uiLabel.color = colorOnMaxMultiplier;
		}
		else
		{
			uiLabel.color = currentNormalColor;
		}
	}

	private void OnUseMultiplierBooster()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		currentNormalColor = colorOnMultiplierBooster;
	}
}
