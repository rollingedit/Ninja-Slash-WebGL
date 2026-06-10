using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShieldFade : MonoBehaviour
{
	public TweenAlpha tweenAlpha;

	private void OnGetObjectFromPool()
	{
		tweenAlpha.Reset();
		tweenAlpha.Play(true);
		NGUIUtility.DestroyWhenTweenFinished(((Component)this).gameObject, tweenAlpha);
	}
}
