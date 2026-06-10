using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Sound Volume")]
[RequireComponent(typeof(UISlider))]
public class UISoundVolume : MonoBehaviour
{
	private UISlider mSlider;

	private void Awake()
	{
		mSlider = ((Component)this).GetComponent<UISlider>();
		mSlider.sliderValue = NGUITools.soundVolume;
		mSlider.eventReceiver = ((Component)this).gameObject;
	}

	private void OnSliderChange(float val)
	{
		NGUITools.soundVolume = val;
	}
}
