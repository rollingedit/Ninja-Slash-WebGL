using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PauseWindow : MonoBehaviour
{
	public TweenPosition tweenPosition;

	public GameObject missionPanel;

	public GameObject panelAnchor;

	private GameObject panelInst;

	private void OnGetObjectFromPool()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		tweenPosition.Play(true);
		panelInst = PoolManager.SpawnAndAttachToParent(missionPanel, panelAnchor);
		panelInst.transform.localPosition = Vector3.zero;
		panelInst.transform.localScale = Vector3.one;
	}

	private void OnPutObjectIntoPool()
	{
		PoolManager.Despawn(panelInst);
	}

	private void OnDoResume()
	{
		tweenPosition.Play(false);
		NGUIUtility.DestroyWhenTweenFinished(((Component)this).gameObject, tweenPosition);
	}

	private void OnHomeButtonClicked()
	{
		tweenPosition.Play(false);
		NGUIUtility.DestroyWhenTweenFinished(((Component)this).gameObject, tweenPosition);
	}
}
