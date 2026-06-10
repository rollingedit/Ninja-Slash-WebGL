using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BuffIndicator : MonoBehaviour
{
	public ScrollElement element;

	public UpdateTimeTarget target;

	public UISprite spriteOnActive;

	public TweenAlpha tweenAlpha;

	public UISprite spin;

	private bool almostFinished;

	private float time;

	private void OnEnable()
	{
		spriteOnActive.alpha = 0f;
		almostFinished = false;
	}

	private void OnUpdateTime(UpdateTimeData dataIn)
	{
		if (dataIn.target == target)
		{
			time = dataIn.curValue;
			if (time <= 3f && !almostFinished)
			{
				almostFinished = true;
				tweenAlpha.Play(true);
			}
		}
	}

	private void OnStartBuff(ScrollElement elementIn)
	{
		if (element == elementIn)
		{
			almostFinished = false;
			((Behaviour)tweenAlpha).enabled = false;
			tweenAlpha.Reset();
			spriteOnActive.alpha = 1f;
			spin.alpha = 1f;
			((Component)spin).gameObject.GetComponent<Animation>().Play();
		}
	}

	private void OnFinishBuff(ScrollElement elementIn)
	{
		if (element == elementIn)
		{
			almostFinished = false;
			((Behaviour)tweenAlpha).enabled = false;
			tweenAlpha.Reset();
			spin.alpha = 0f;
			((Component)spin).gameObject.GetComponent<Animation>().Stop();
		}
	}
}
