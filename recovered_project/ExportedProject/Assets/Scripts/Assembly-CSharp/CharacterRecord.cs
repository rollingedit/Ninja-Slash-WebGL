using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterRecord : MonoBehaviour
{
	private int score;

	private int coin;

	private int combo;

	private int kills;

	public int gameCountForAskingReview = 10;

	private void OnGetObjectFromPool()
	{
		score = 0;
		coin = 0;
		combo = 0;
		kills = 0;
	}

	public void AddScore(int scoreIn)
	{
		score += scoreIn;
		MonoSingleton<MissionManager>.instance.ProcessMission(Mission.Type.score, scoreIn);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGetScoreEvent(score));
	}

	public void AddCoin(int coinIn)
	{
		coin += coinIn;
		MonoSingleton<UserData>.instance.AddCoin(coinIn);
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGetCoinEvent(coinIn));
	}

	public void AddCombo(int comboIn)
	{
		if (combo < comboIn)
		{
			combo = comboIn;
		}
	}

	public void IncreamentKill()
	{
		kills++;
		MonoSingleton<UserData>.instance.IncreamentKill();
	}

	private void OnGameOver()
	{
		bool flag = score > MonoSingleton<UserData>.instance.HighScore;
		if (flag)
		{
			MonoSingleton<UserData>.instance.HighScore = score;
		}
		if (MonoSingleton<UserData>.instance.HighCombo < combo)
		{
			MonoSingleton<UserData>.instance.HighCombo = combo;
		}
		MonoSingleton<UserData>.instance.PlayedGameCount++;
		if (MonoSingleton<UserData>.instance.PlayedGameCount == gameCountForAskingReview)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnRateThisAppEvent());
		}
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnGameOverEvent(new GameOverResult(score, coin, combo, kills, flag)));
	}
}
