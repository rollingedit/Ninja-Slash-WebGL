using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class KongregateControl : MonoBehaviour
{
	private bool isConnected;

	private string scoreStatName = "score";

	private string comboStatName = "combo";

	private string totalKill = "total_kill";

	private void Start()
	{
		Debug.Log((object)"Start");
		Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined'){ kongregateUnitySupport.initAPI('" + ((Object)((Component)this).gameObject).name + "', 'OnKongregateAPILoaded');};");
	}

	private void OnKongregateAPILoaded(string userInfoString)
	{
		isConnected = true;
		string[] array = userInfoString.Split(new char[1] { "|"[0] });
		int num = int.Parse(array[0]);
		string text = array[1];
		string text2 = array[2];
	}

	private void UpdateStats(int scoreIn, int comboIn, int killIn)
	{
		Send(scoreStatName, scoreIn);
		Send(comboStatName, comboIn);
		Send(totalKill, killIn);
	}

	private void Send(string statName, int val)
	{
		Debug.Log((object)("Submit to Kongregate : " + statName + " - " + val));
		if (isConnected)
		{
			Application.ExternalCall("kongregate.stats.submit", new object[2] { statName, val });
		}
	}

	private void OnGameOver(GameOverResult resultIn)
	{
		UpdateStats(resultIn.Score, resultIn.Combo, resultIn.Kill);
	}

	private void OnKilledZombie()
	{
	}
}
