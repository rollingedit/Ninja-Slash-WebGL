using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TutorialArrow : MonoBehaviour
{
	public TweenPosition tweenPosition;

	public TweenAlpha tweenAlpha;

	public TweenScale tweenScale;

	private void OnGetObjectFromPool()
	{
		tweenPosition.Play(true);
		tweenAlpha.Play(true);
		tweenScale.Play(true);
	}

	private void OnPutObjectIntoPool()
	{
		tweenPosition.Reset();
		tweenAlpha.Reset();
		tweenScale.Reset();
	}
}
