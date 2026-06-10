using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ArrangeRigidbody : MonoBehaviour
{
	public bool dontPool;

	public float poolDelay = 1f;

	private List<Transform> children;

	private List<Vector3> childPos;

	private List<Quaternion> childRot;

	private void Awake()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		children = new List<Transform>();
		Utility.GetRigidbodyRecursively(((Component)this).transform, children);
		childPos = new List<Vector3>();
		childRot = new List<Quaternion>();
		var enumerator = children.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = enumerator.Current;
				childPos.Add(current.localPosition);
				childRot.Add(current.localRotation);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void OnGetObjectFromPool()
	{
		if (!dontPool)
		{
			PoolManager.Instance.DespawnAfterDelay(((Component)this).gameObject, poolDelay);
		}
	}

	private void OnPutObjectIntoPool()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		var enumerator = children.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = enumerator.Current;
				((Component)current).GetComponent<Rigidbody>().velocity = Vector3.zero;
				current.localPosition = childPos[num];
				current.localRotation = childRot[num];
				num++;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}
}
