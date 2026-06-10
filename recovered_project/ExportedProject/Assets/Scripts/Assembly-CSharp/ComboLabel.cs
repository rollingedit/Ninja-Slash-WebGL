using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ComboLabel : BaseLabel
{
	public UILabel uiLabel;

	public TweenScale tweenScale;

	public float maxAlphaTime;

	public GameObject comboLabel;

	private int combo;

	private float alphaValueMultiplier;

	private void Awake()
	{
		alphaValueMultiplier = 1f / (1f - maxAlphaTime);
	}

	public override void Initialize()
	{
		combo = 0;
		UpdateLabel();
		comboLabel.SetActive(false);
	}

	public override void UpdateLabel()
	{
		if (combo == 0)
		{
			uiLabel.text = string.Empty;
		}
		else
		{
			uiLabel.text = combo.ToString();
		}
	}

	private void OnCombo(int comboIn)
	{
		if (comboIn == 0)
		{
			Initialize();
			return;
		}
		if (combo == 0)
		{
			comboLabel.SetActive(true);
		}
		combo = comboIn;
		UpdateLabel();
		tweenScale.Reset();
		tweenScale.Play(true);
	}

	private void OnUpdateTime(UpdateTimeData dataIn)
	{
		if (dataIn.target == UpdateTimeTarget.combo)
		{
			uiLabel.alpha = Mathf.Clamp(dataIn.guageValue * alphaValueMultiplier, 0f, 1f);
		}
	}
}
