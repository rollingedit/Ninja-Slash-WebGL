using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterPanel : MonoBehaviour
{
	public UIDraggablePanel dragPanel;

	public float minSwipeDistancePixel;

	public List<GameObject> characterPanelCells;

	private bool pressStarted;

	private float pressStartPosX;

	private int curIndex;

	private bool initialized;

	private void OnGetObjectFromPool()
	{
		InitializePanel();
	}

	private void OnEnable()
	{
		if (!initialized)
		{
			InitializePanel();
		}
	}

	private void OnPutObjectIntoPool()
	{
		initialized = false;
	}

	private void InitializePanel()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		curIndex = MonoSingleton<UserData>.instance.SelectedNinjaIndex;
		if (characterPanelCells == null || characterPanelCells.Count == 0)
		{
			return;
		}
		curIndex = Mathf.Clamp(curIndex, 0, characterPanelCells.Count - 1);
		MonoSingleton<UserData>.instance.SelectedNinjaIndex = curIndex;
		MoveWindow();
		initialized = true;
		var enumerator = characterPanelCells.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.SendMessage("OnWindowMoved", (object)(characterPanelCells.IndexOf(current) == curIndex));
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}

	private void Update()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		if (Input.GetMouseButtonDown(0))
		{
			pressStarted = true;
			pressStartPosX = Input.mousePosition.x;
		}
		if (!pressStarted || !Input.GetMouseButtonUp(0))
		{
			return;
		}
		pressStarted = false;
		float num = Input.mousePosition.x - pressStartPosX;
		if (Mathf.Abs(num) > minSwipeDistancePixel)
		{
			if (num > 0f)
			{
				curIndex = Mathf.Clamp(curIndex - 1, 0, curIndex);
			}
			else
			{
				curIndex = Mathf.Clamp(curIndex + 1, curIndex, characterPanelCells.Count - 1);
			}
			var enumerator = characterPanelCells.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					current.SendMessage("OnWindowMoved", (object)(characterPanelCells.IndexOf(current) == curIndex));
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
		MoveWindow();
	}

	private void MoveWindow()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)dragPanel == (Object)null || characterPanelCells == null || characterPanelCells.Count == 0)
		{
			return;
		}
		curIndex = Mathf.Clamp(curIndex, 0, characterPanelCells.Count - 1);
		Matrix4x4 worldToLocalMatrix = ((Component)dragPanel).transform.worldToLocalMatrix;
		Vector3 pos = worldToLocalMatrix.MultiplyPoint3x4(characterPanelCells[curIndex].transform.position);
		pos = new Vector3(0f - pos.x, 0f, 0f);
		((Component)dragPanel).transform.localPosition = pos;
		SpringPanel.Begin(((Component)dragPanel).gameObject, pos, 6f);
	}

	private void OnClickSelectButton()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (MonoSingleton<UserData>.instance.SelectedNinjaIndex == curIndex)
		{
			return;
		}
		MonoSingleton<UserData>.instance.SelectedNinjaIndex = curIndex;
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangeModelEvent());
		var enumerator = characterPanelCells.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.SendMessage("OnModelChanged");
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
	}
}
