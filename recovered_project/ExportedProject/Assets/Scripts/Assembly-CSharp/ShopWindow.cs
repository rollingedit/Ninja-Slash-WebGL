using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ShopWindow : MonoBehaviour
{
	public GameObject upgradesPanel;

	public GameObject coinStorePanel;

	public GameObject panelAnchor;

	private ShopPanelType curPanelType;

	private GameObject curPanel;

	private void OnGetObjectFromPool()
	{
		curPanelType = ShopPanelType.upgrades;
		ClearPanelAnchor();
		curPanel = GetCurrentPanel();
		((Component)this).BroadcastMessage("OnSetShopPanelButton", (object)curPanelType, (SendMessageOptions)1);
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
		return (Object)(object)poolObject != (Object)null && (poolObject.PrefabName == "upgrades_panel" || poolObject.PrefabName == "coin_store_panel");
	}

	private GameObject GetCurrentPanel()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = null;
		if (curPanelType == ShopPanelType.upgrades)
		{
			val = PoolManager.SpawnAndAttachToParent(upgradesPanel, panelAnchor);
		}
		else if (curPanelType == ShopPanelType.coinStore)
		{
			val = PoolManager.SpawnAndAttachToParent(coinStorePanel, panelAnchor);
		}
		val.transform.localPosition = Vector3.zero;
		val.transform.localScale = Vector3.one;
		return val;
	}

	private void OnClickButton(ShopPanelType typeIn)
	{
		if (curPanelType != typeIn)
		{
			PoolManager.Despawn(curPanel);
			curPanel = null;
			ClearPanelAnchor();
			curPanelType = typeIn;
			curPanel = GetCurrentPanel();
			((Component)this).BroadcastMessage("OnSetShopPanelButton", (object)curPanelType);
		}
	}
}
