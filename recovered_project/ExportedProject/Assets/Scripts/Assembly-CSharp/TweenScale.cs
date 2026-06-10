using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Tween/Scale")]
public class TweenScale : UITweener
{
	public Vector3 from = Vector3.one;

	public Vector3 to = Vector3.one;

	public bool updateTable;

	private Transform mTrans;

	private UITable mTable;

	public Transform cachedTransform
	{
		get
		{
			if ((Object)(object)mTrans == (Object)null)
			{
				mTrans = ((Component)this).transform;
			}
			return mTrans;
		}
	}

	public Vector3 scale
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return cachedTransform.localScale;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			cachedTransform.localScale = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		cachedTransform.localScale = from * (1f - factor) + to * factor;
		if (!updateTable)
		{
			return;
		}
		if ((Object)(object)mTable == (Object)null)
		{
			mTable = NGUITools.FindInParents<UITable>(((Component)this).gameObject);
			if ((Object)(object)mTable == (Object)null)
			{
				updateTable = false;
				return;
			}
		}
		mTable.repositionNow = true;
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.scale;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			((Behaviour)tweenScale).enabled = false;
		}
		return tweenScale;
	}
}
