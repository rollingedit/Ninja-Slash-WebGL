using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TimeScaleControl : MonoBehaviour
{
	public float resumeTime;

	private bool isPaused;

	private float resumeLeftTime;

	private float realTime;

	private float targetTimeScale;

	private bool isGameStarted;

	private bool isInterpolating;

	private void Update()
	{
		if (!isPaused && isInterpolating)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, 10f * Time.deltaTime);
			if (Mathf.Abs(Time.timeScale - targetTimeScale) < 0.1f)
			{
				Time.timeScale = targetTimeScale;
				isInterpolating = false;
			}
		}
		if (resumeLeftTime > 0f)
		{
			float num = Time.realtimeSinceStartup - realTime;
			resumeLeftTime = Mathf.Clamp(resumeLeftTime - num, 0f, resumeLeftTime);
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnResumeCountEvent(resumeLeftTime));
			if (resumeLeftTime == 0f)
			{
				isPaused = false;
				Time.timeScale = 1f;
				MonoSingleton<EventManager>.instance.TriggerEvent(new OnResumeEvent());
			}
		}
		realTime = Time.realtimeSinceStartup;
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator ChangeTimeScale(float timeScaleIn, float durationIn)
	{
		targetTimeScale = timeScaleIn;
		isInterpolating = true;
		yield return (object)new WaitForSeconds(durationIn);
		targetTimeScale = 1f;
		isInterpolating = true;
	}

	private void OnChangeTimeScale(ChangeTimeScaleData timeScaleDataIn)
	{
		((MonoBehaviour)this).StartCoroutine(ChangeTimeScale(timeScaleDataIn.TimeScale, timeScaleDataIn.Duration));
	}

	private void OnPause()
	{
		isPaused = true;
		Time.timeScale = 0f;
	}

	private void OnDoResume()
	{
		((MonoBehaviour)this).StartCoroutine("DoResume");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DoResume()
	{
		yield return 0;
		resumeLeftTime = resumeTime;
	}

	private void OnHome()
	{
		if (isPaused)
		{
			Time.timeScale = 1f;
			isPaused = false;
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnResumeEvent());
		}
		isGameStarted = false;
	}

	private void OnGameStart()
	{
		isGameStarted = true;
	}

	private void OnGameOver()
	{
		isGameStarted = false;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (isGameStarted && !isPaused)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnPauseEvent());
		}
	}
}
