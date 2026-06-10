using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal = 0,
		Vertical = 1
	}

	public Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool repositionNow;

	public bool sorted;

	public bool hideInactive = true;

	private bool mStarted;

	private void Start()
	{
		mStarted = true;
		Reposition();
	}

	private void Update()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(((Object)a).name, ((Object)b).name);
	}

	public void Reposition()
	{
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (!mStarted)
		{
			repositionNow = true;
			return;
		}
		Transform transform = ((Component)this).transform;
		int num = 0;
		int num2 = 0;
		if (sorted)
		{
			List<Transform> val = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (((Object)(object)child != (Object)null) && (!hideInactive || NGUITools.GetActive(((Component)child).gameObject)))
				{
					val.Add(child);
				}
			}
			val.Sort((Comparison<Transform>)SortByName);
			int j = 0;
			for (int count = val.Count; j < count; j++)
			{
				Transform val2 = val[j];
				if (NGUITools.GetActive(((Component)val2).gameObject) || !hideInactive)
				{
					float z = val2.localPosition.z;
					val2.localPosition = ((arrangement != Arrangement.Horizontal) ? new Vector3(cellWidth * (float)num2, (0f - cellHeight) * (float)num, z) : new Vector3(cellWidth * (float)num, (0f - cellHeight) * (float)num2, z));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child2 = transform.GetChild(k);
				if (NGUITools.GetActive(((Component)child2).gameObject) || !hideInactive)
				{
					float z2 = child2.localPosition.z;
					child2.localPosition = ((arrangement != Arrangement.Horizontal) ? new Vector3(cellWidth * (float)num2, (0f - cellHeight) * (float)num, z2) : new Vector3(cellWidth * (float)num, (0f - cellHeight) * (float)num2, z2));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		UIDraggablePanel uIDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(((Component)this).gameObject);
		if ((Object)(object)uIDraggablePanel != (Object)null)
		{
			uIDraggablePanel.UpdateScrollbars(true);
		}
	}
}
