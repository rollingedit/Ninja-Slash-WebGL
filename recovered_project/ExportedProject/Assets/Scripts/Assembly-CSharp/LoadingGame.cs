using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class LoadingGame : MonoBehaviour
{
	private AsyncOperation async;

	public UISlider uiSlider;

	public List<GameObject> objectsToDestroyOnLoadComplete;

	private void Start()
	{
		Application.backgroundLoadingPriority = (ThreadPriority)4;
		((MonoBehaviour)this).StartCoroutine("LoadScene");
	}

	private void Update()
	{
		if (!async.isDone)
		{
			uiSlider.sliderValue = async.progress;
		}
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator LoadScene()
	{
		async = Application.LoadLevelAdditiveAsync("gameScene");
		yield return async;
		var enumerator = objectsToDestroyOnLoadComplete.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject go = enumerator.Current;
				Object.Destroy((Object)(object)go);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	public void Main()
	{
	}
}
