using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class AppStoreLinkButton : MonoBehaviour
{
	public string address;

	public TweenPosition tween;

	private void Awake()
	{
		if (string.IsNullOrEmpty(address))
		{
			address = "https://github.com/rollingedit/Ninja-Slash-WebGL";
		}
	}

	private void OnClick()
	{
		Application.OpenURL(address);
	}

	private void OnGameStart()
	{
		if ((Object)(object)tween != (Object)null)
		{
			tween.Play(false);
		}
	}

	private void OnHome()
	{
		if ((Object)(object)tween != (Object)null)
		{
			tween.Play(true);
		}
	}
}
