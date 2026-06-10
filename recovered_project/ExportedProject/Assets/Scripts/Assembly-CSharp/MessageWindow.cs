using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MessageWindow : MonoBehaviour
{
	public UITweener tween;

	private void OnGetObjectFromPool()
	{
		tween.Play(true);
	}

	public void OnShowTimeFinished()
	{
		tween.Play(false);
		NGUIUtility.DestroyWhenTweenFinished(((Component)this).gameObject, tween);
	}
}
