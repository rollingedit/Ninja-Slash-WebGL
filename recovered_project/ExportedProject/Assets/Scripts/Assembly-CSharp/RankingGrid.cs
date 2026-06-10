using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class RankingGrid : MonoBehaviour
{
	public UIGrid uiGrid;

	public GameObject rankingElementPrefab;

	public GameObject rankingElementMePrefab;

	public GameObject recommendPrefab;

	private List<GameObject> childList = new List<GameObject>();

	private void OnGetFacebookRank(List<FacebookUser> users)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		childList.Clear();
		for (int i = 0; i < ((Component)this).transform.childCount; i++)
		{
			childList.Add(((Component)((Component)this).transform.GetChild(i)).gameObject);
		}
		var enumerator = childList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				PoolManager.Despawn(current);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		int num = 0;
		var enumerator2 = users.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				FacebookUser current2 = enumerator2.Current;
				num++;
				GameObject prefabIn = ((!current2.IsMe) ? rankingElementPrefab : rankingElementMePrefab);
				GameObject val = NGUIUtility.AddChild(((Component)this).gameObject, prefabIn);
				val.SendMessage("SetUser", (object)current2);
				val.SendMessage("SetRank", (object)num);
				((Object)val).name = string.Format("{0:0000000000}", (object)num);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2).Dispose();
		}
		NGUIUtility.AddChild(((Component)this).gameObject, recommendPrefab);
		((MonoBehaviour)this).StartCoroutine("WaitAndReposition");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndReposition()
	{
		yield return (object)new WaitForEndOfFrame();
		uiGrid.Reposition();
	}

	private void OnWindowTweenFinished()
	{
		uiGrid.Reposition();
	}
}
