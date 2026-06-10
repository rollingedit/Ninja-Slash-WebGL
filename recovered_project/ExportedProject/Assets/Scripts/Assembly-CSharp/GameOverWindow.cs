using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class GameOverWindow : MonoBehaviour
{
	public TweenPosition tweenPosition;

	public AudioClip gameoverClip;

	private void OnGameOver(GameOverResult resultIn)
	{
		tweenPosition.Play(true);
		MonoSingleton<SoundManager>.instance.PlaySound(gameoverClip);
	}

	private void OnRestart()
	{
		tweenPosition.Play(false);
	}

	private void OnHomeButtonClicked()
	{
		tweenPosition.Play(false);
	}
}
