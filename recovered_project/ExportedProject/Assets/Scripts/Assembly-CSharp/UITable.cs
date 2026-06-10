using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : MonoBehaviour
{
	public enum Direction
	{
		Down = 0,
		Up = 1
	}

	public delegate void OnReposition();

	public int columns;

	public Direction direction;

	public Vector2 padding = Vector2.zero;

	public bool sorted;

	public bool hideInactive = true;

	public bool repositionNow;

	public bool keepWithinPanel;

	public OnReposition onReposition;

	private UIPanel mPanel;

	private UIDraggablePanel mDrag;

	private bool mStarted;

	private List<Transform> mChildren = new List<Transform>();

	public List<Transform> children
	{
		get
		{
			if (mChildren.Count == 0)
			{
				Transform transform = ((Component)this).transform;
				mChildren.Clear();
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					if (((Object)(object)child != (Object)null) && ((Object)(object)((Component)child).gameObject != (Object)null) && (!hideInactive || NGUITools.GetActive(((Component)child).gameObject)))
					{
						mChildren.Add(child);
					}
				}
				if (sorted)
				{
					mChildren.Sort((Comparison<Transform>)SortByName);
				}
			}
			return mChildren;
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(((Object)a).name, ((Object)b).name);
	}

	private void RepositionVariableSize(List<Transform> children)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		int num3 = ((columns <= 0) ? 1 : (children.Count / columns + 1));
		int num4 = ((columns <= 0) ? children.Count : columns);
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = (Bounds[])(object)new Bounds[num4];
		Bounds[] array3 = (Bounds[])(object)new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Transform val = children[i];
			Bounds val2 = NGUIMath.CalculateRelativeWidgetBounds(val);
			Vector3 localScale = val.localScale;
			val2.min = Vector3.Scale(val2.min, localScale);
			val2.max = Vector3.Scale(val2.max, localScale);
			array[num6, num5] = val2;
			array2[num5].Encapsulate(val2);
			array3[num6].Encapsulate(val2);
			if (++num5 >= columns && columns > 0)
			{
				num5 = 0;
				num6++;
			}
		}
		num5 = 0;
		num6 = 0;
		int j = 0;
		for (int count2 = children.Count; j < count2; j++)
		{
			Transform val3 = children[j];
			Bounds val4 = array[num6, num5];
			Bounds val5 = array2[num5];
			Bounds val6 = array3[num6];
			Vector3 localPosition = val3.localPosition;
			localPosition.x = num + val4.extents.x - val4.center.x;
			localPosition.x += val4.min.x - val5.min.x + padding.x;
			if (direction == Direction.Down)
			{
				localPosition.y = 0f - num2 - val4.extents.y - val4.center.y;
				localPosition.y += (val4.max.y - val4.min.y - val6.max.y + val6.min.y) * 0.5f - padding.y;
			}
			else
			{
				localPosition.y = num2 + val4.extents.y - val4.center.y;
				localPosition.y += (val4.max.y - val4.min.y - val6.max.y + val6.min.y) * 0.5f - padding.y;
			}
			num += val5.max.x - val5.min.x + padding.x * 2f;
			val3.localPosition = localPosition;
			if (++num5 >= columns && columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += val6.size.y + padding.y * 2f;
			}
		}
	}

	public void Reposition()
	{
		if (mStarted)
		{
			Transform transform = ((Component)this).transform;
			mChildren.Clear();
			List<Transform> val = children;
			if (val.Count > 0)
			{
				RepositionVariableSize(val);
			}
			if ((Object)(object)mDrag != (Object)null)
			{
				mDrag.UpdateScrollbars(true);
				mDrag.RestrictWithinBounds(true);
			}
			else if ((Object)(object)mPanel != (Object)null)
			{
				mPanel.ConstrainTargetToBounds(transform, true);
			}
			if (onReposition != null)
			{
				onReposition();
			}
		}
		else
		{
			repositionNow = true;
		}
	}

	private void Start()
	{
		mStarted = true;
		if (keepWithinPanel)
		{
			mPanel = NGUITools.FindInParents<UIPanel>(((Component)this).gameObject);
			mDrag = NGUITools.FindInParents<UIDraggablePanel>(((Component)this).gameObject);
		}
		Reposition();
	}

	private void LateUpdate()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}
}
