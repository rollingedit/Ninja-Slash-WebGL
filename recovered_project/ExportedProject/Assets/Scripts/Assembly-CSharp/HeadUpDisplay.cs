using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class HeadUpDisplay : MonoBehaviour
{
	public List<GameObject> objectsToActivateOnGameStart;

	public List<GameObject> objectsToDeactivateOnGameStart;

	private void OnGameStart()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = objectsToActivateOnGameStart.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.SetActive(true);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		var enumerator2 = objectsToDeactivateOnGameStart.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				GameObject current2 = enumerator2.Current;
				current2.SetActive(false);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2).Dispose();
		}
	}

	private void OnHome()
	{
		((MonoBehaviour)this).StartCoroutine("WaitAndHome");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndHome()
	{
		yield return 0;
		var enumerator = objectsToActivateOnGameStart.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject go = enumerator.Current;
				go.SetActive(false);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		var enumerator2 = objectsToDeactivateOnGameStart.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				GameObject go2 = enumerator2.Current;
				go2.SetActive(true);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2).Dispose();
		}
	}
}
