using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MultiplierGuage : MonoBehaviour
{
	public UISlider uiSlider;

	public ChangeSprite foreground;

	public GameObject backgroundObject;

	public ChangeSprite backgroundNormal;

	public UISprite backgroundFlame;

	public SpriteData backSpriteOnWind;

	public SpriteData frontSpriteOnWind;

	private void OnEnable()
	{
		SetGuageValue(0f);
	}

	private void OnChangeMultiplier(MultiplierData dataIn)
	{
		SetGuageValue(dataIn.guageValue);
	}

	private void OnGameOver()
	{
		SetGuageValue(0f);
	}

	private void SetGuageValue(float guageValue)
	{
		if (guageValue == 0f)
		{
			backgroundObject.SetActive(false);
		}
		else if (uiSlider.sliderValue == 0f)
		{
			backgroundObject.SetActive(true);
		}
		uiSlider.sliderValue = guageValue;
	}

	private void OnStartBuff(ScrollElement element)
	{
		switch (element)
		{
		case ScrollElement.Flame:
			((Component)backgroundFlame).gameObject.SetActive(true);
			break;
		case ScrollElement.Wind:
			backgroundNormal.OnChangeSprite(backSpriteOnWind);
			foreground.OnChangeSprite(frontSpriteOnWind);
			break;
		}
	}

	private void OnFinishBuff(ScrollElement element)
	{
		switch (element)
		{
		case ScrollElement.Flame:
			((Component)backgroundFlame).gameObject.SetActive(false);
			break;
		case ScrollElement.Wind:
			backgroundNormal.OnRestoreSprite();
			foreground.OnRestoreSprite();
			break;
		}
	}
}
