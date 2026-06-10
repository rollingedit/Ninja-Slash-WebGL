using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class UINode
{
	private int mVisibleFlag = -1;

	public Transform trans;

	public UIWidget widget;

	public bool lastActive;

	public Vector3 lastPos;

	public Quaternion lastRot;

	public Vector3 lastScale;

	public int changeFlag = -1;

	private GameObject mGo;

	public int visibleFlag
	{
		get
		{
			return (!((Object)(object)widget != (Object)null)) ? mVisibleFlag : widget.visibleFlag;
		}
		set
		{
			if ((Object)(object)widget != (Object)null)
			{
				widget.visibleFlag = value;
			}
			else
			{
				mVisibleFlag = value;
			}
		}
	}

	public UINode(Transform t)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		trans = t;
		lastPos = trans.localPosition;
		lastRot = trans.localRotation;
		lastScale = trans.localScale;
		mGo = ((Component)t).gameObject;
	}

	public bool HasChanged()
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		bool flag = NGUITools.GetActive(mGo) && ((Object)(object)widget == (Object)null || (((Behaviour)widget).enabled && widget.color.a > 0.001f));
		if (lastActive != flag || (flag && (lastPos != trans.localPosition || lastRot != trans.localRotation || lastScale != trans.localScale)))
		{
			lastActive = flag;
			lastPos = trans.localPosition;
			lastRot = trans.localRotation;
			lastScale = trans.localScale;
			return true;
		}
		return false;
	}
}
