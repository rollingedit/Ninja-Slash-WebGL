using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MainMenuWindow : MonoBehaviour
{
	public TweenPosition topTweenPosition;

	public TweenPosition bottomTweenPosition;

	private void OnTapToStartButtonClicked()
	{
		bottomTweenPosition.Play(true);
		topTweenPosition.Play(true);
	}

	private void OnHome()
	{
		bottomTweenPosition.Play(false);
		topTweenPosition.Play(false);
	}
}
