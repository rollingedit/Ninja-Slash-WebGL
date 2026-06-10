using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShieldIndicator : MonoBehaviour
{
	public UISlider uiSlider;

	public GameObject coolTimeSprite;

	public UISprite spinSprite;

	public UILabel shieldCountLabel;

	private void OnEnable()
	{
		((Component)uiSlider).gameObject.SetActive(false);
		SetCountLabel();
	}

	private void OnShieldStart()
	{
		((Component)uiSlider).gameObject.SetActive(true);
		spinSprite.alpha = 1f;
		((Component)spinSprite).gameObject.GetComponent<Animation>().Play();
		SetCountLabel();
		((Component)shieldCountLabel).gameObject.SetActive(false);
	}

	private void OnShieldFinish()
	{
		((Component)uiSlider).gameObject.SetActive(false);
		spinSprite.alpha = 0f;
		((Component)spinSprite).gameObject.GetComponent<Animation>().Stop();
	}

	private void OnUpdateTime(UpdateTimeData dataIn)
	{
		if (dataIn.target == UpdateTimeTarget.shield)
		{
			uiSlider.sliderValue = dataIn.guageValue;
		}
	}

	private void OnShieldCoolTimeStart()
	{
		coolTimeSprite.SetActive(true);
	}

	private void OnShieldCoolTimeFinish()
	{
		coolTimeSprite.SetActive(false);
		((Component)shieldCountLabel).gameObject.SetActive(true);
	}

	private void OnItemNumberChanged()
	{
		SetCountLabel();
	}

	private void SetCountLabel()
	{
		shieldCountLabel.text = MonoSingleton<UserData>.instance.ShieldCount.ToString();
	}
}
