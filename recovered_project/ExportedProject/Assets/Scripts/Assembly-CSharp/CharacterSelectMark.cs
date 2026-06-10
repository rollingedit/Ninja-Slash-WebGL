using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterSelectMark : MonoBehaviour
{
	public UISprite uiSprite;

	public TweenScale tweenScale;

	private void OnWindowMoved(bool isFocused)
	{
		OnSetMark(isFocused);
	}

	private void OnSetMark(bool isFocused)
	{
		if (isFocused)
		{
			uiSprite.alpha = 1f;
			tweenScale.Play(true);
		}
		else
		{
			uiSprite.alpha = 20f / 51f;
			tweenScale.Play(false);
		}
	}
}
