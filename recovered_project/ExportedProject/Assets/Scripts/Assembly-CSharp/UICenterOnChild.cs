using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Center On Child")]
public class UICenterOnChild : MonoBehaviour
{
	public SpringPanel.OnFinished onFinished;

	private UIDraggablePanel mDrag;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return mCenteredObject;
		}
	}

	private void OnEnable()
	{
		Recenter();
	}

	private void OnDragFinished()
	{
		if (((Behaviour)this).enabled)
		{
			Recenter();
		}
	}

	public void Recenter()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mDrag == (Object)null)
		{
			mDrag = NGUITools.FindInParents<UIDraggablePanel>(((Component)this).gameObject);
			if ((Object)(object)mDrag == (Object)null)
			{
				Debug.LogWarning((object)string.Concat(new object[4]
				{
					((object)this).GetType(),
					" requires ",
					typeof(UIDraggablePanel),
					" on a parent object in order to work"
				}), (Object)(object)this);
				((Behaviour)this).enabled = false;
				return;
			}
			mDrag.onDragFinished = OnDragFinished;
		}
		if ((Object)(object)mDrag.panel == (Object)null)
		{
			return;
		}
		Vector4 clipRange = mDrag.panel.clipRange;
		Transform cachedTransform = mDrag.panel.cachedTransform;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.x += clipRange.x;
		localPosition.y += clipRange.y;
		localPosition = cachedTransform.parent.TransformPoint(localPosition);
		Vector3 val = localPosition - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
		mDrag.currentMomentum = Vector3.zero;
		float num = 3.4028235E+38f;
		Transform val2 = null;
		Transform transform = ((Component)this).transform;
		int i = 0;
		for (int childCount = transform.childCount; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			float num2 = Vector3.SqrMagnitude(child.position - val);
			if (num2 < num)
			{
				num = num2;
				val2 = child;
			}
		}
		if ((Object)(object)val2 != (Object)null)
		{
			mCenteredObject = ((Component)val2).gameObject;
			Vector3 val3 = cachedTransform.InverseTransformPoint(val2.position);
			Vector3 val4 = cachedTransform.InverseTransformPoint(localPosition);
			Vector3 val5 = val3 - val4;
			if (mDrag.scale.x == 0f)
			{
				val5.x = 0f;
			}
			if (mDrag.scale.y == 0f)
			{
				val5.y = 0f;
			}
			if (mDrag.scale.z == 0f)
			{
				val5.z = 0f;
			}
			SpringPanel.Begin(((Component)mDrag).gameObject, cachedTransform.localPosition - val5, 8f).onFinished = onFinished;
		}
		else
		{
			mCenteredObject = null;
		}
	}
}
