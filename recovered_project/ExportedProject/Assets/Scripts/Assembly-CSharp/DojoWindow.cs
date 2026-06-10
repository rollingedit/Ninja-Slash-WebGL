using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class DojoWindow : MonoBehaviour
{
	public GameObject ninjaPanel;

	public GameObject missionPanel;

	public GameObject statPanel;

	public GameObject panelAnchor;

	private DojoPanelType curPanelType;

	private GameObject curPanel;

	private void OnGetObjectFromPool()
	{
		curPanelType = DojoPanelType.ninja;
		ClearPanelAnchor();
		curPanel = GetCurrentPanel();
		((Component)this).BroadcastMessage("OnSetDojoPanelButton", (object)curPanelType, (SendMessageOptions)1);
	}

	private void OnPutObjectIntoPool()
	{
		PoolManager.Despawn(curPanel);
		curPanel = null;
		ClearPanelAnchor();
	}

	private void ClearPanelAnchor()
	{
		if ((Object)(object)panelAnchor == (Object)null)
		{
			return;
		}
		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in panelAnchor.transform)
		{
			children.Add(((Component)child).gameObject);
		}
		for (int i = 0; i < children.Count; i++)
		{
			if (IsDynamicPanel(children[i]))
			{
				PoolManager.Despawn(children[i]);
			}
		}
	}

	private bool IsDynamicPanel(GameObject go)
	{
		PoolObject poolObject = go.GetComponent<PoolObject>();
		return (Object)(object)poolObject != (Object)null && (poolObject.PrefabName == "character_panel" || poolObject.PrefabName == "mission_panel" || poolObject.PrefabName == "stat_panel");
	}

	private GameObject GetCurrentPanel()
	{
		GameObject val = null;
		if (curPanelType == DojoPanelType.ninja)
		{
			val = PoolManager.SpawnAndAttachToParent(ninjaPanel, panelAnchor);
		}
		else if (curPanelType == DojoPanelType.mission)
		{
			val = PoolManager.SpawnAndAttachToParent(missionPanel, panelAnchor);
		}
		else if (curPanelType == DojoPanelType.stat)
		{
			val = PoolManager.SpawnAndAttachToParent(statPanel, panelAnchor);
		}
		val.transform.localPosition = Vector3.zero;
		val.transform.localScale = Vector3.one;
		return val;
	}

	private void OnClickButton(DojoPanelType typeIn)
	{
		if (curPanelType != typeIn)
		{
			PoolManager.Despawn(curPanel);
			curPanel = null;
			ClearPanelAnchor();
			curPanelType = typeIn;
			curPanel = GetCurrentPanel();
			((Component)this).BroadcastMessage("OnSetDojoPanelButton", (object)curPanelType, (SendMessageOptions)1);
		}
	}
}
