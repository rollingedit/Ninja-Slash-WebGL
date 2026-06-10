using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Draggable Panel")]
[RequireComponent(typeof(UIPanel))]
public class UIDraggablePanel : IgnoreTimeScale
{
	public delegate void OnDragFinished();

	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public enum ShowCondition
	{
		Always = 0,
		OnlyIfNeeded = 1,
		WhenDragging = 2
	}

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public float momentumAmount = 35f;

	public Vector2 relativePositionOnReset = Vector2.zero;

	public bool repositionClipping;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;

	public OnDragFinished onDragFinished;

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private int mDragID = -10;

	public UIPanel panel
	{
		get
		{
			return mPanel;
		}
	}

	public Bounds bounds
	{
		get
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			return mBounds;
		}
	}

	public bool shouldMoveHorizontally
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Bounds val = bounds;
			float num = val.size.x;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.x * 2f;
			}
			return num > mPanel.clipRange.z;
		}
	}

	public bool shouldMoveVertically
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Bounds val = bounds;
			float num = val.size.y;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.y * 2f;
			}
			return num > mPanel.clipRange.w;
		}
	}

	private bool shouldMove
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			if (!disableDragIfFits)
			{
				return true;
			}
			if ((Object)(object)mPanel == (Object)null)
			{
				mPanel = ((Component)this).GetComponent<UIPanel>();
			}
			Vector4 clipRange = mPanel.clipRange;
			Bounds val = bounds;
			float num = ((clipRange.z != 0f) ? (clipRange.z * 0.5f) : ((float)Screen.width));
			float num2 = ((clipRange.w != 0f) ? (clipRange.w * 0.5f) : ((float)Screen.height));
			if (!Mathf.Approximately(scale.x, 0f))
			{
				if (val.min.x < clipRange.x - num)
				{
					return true;
				}
				if (val.max.x > clipRange.x + num)
				{
					return true;
				}
			}
			if (!Mathf.Approximately(scale.y, 0f))
			{
				if (val.min.y < clipRange.y - num2)
				{
					return true;
				}
				if (val.max.y > clipRange.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mMomentum;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			mMomentum = value;
			mShouldMove = true;
		}
	}

	private void Awake()
	{
		mTrans = ((Component)this).transform;
		mPanel = ((Component)this).GetComponent<UIPanel>();
	}

	private void Start()
	{
		UpdateScrollbars(true);
		if ((Object)(object)horizontalScrollBar != (Object)null)
		{
			UIScrollBar uIScrollBar = horizontalScrollBar;
			uIScrollBar.onChange = (UIScrollBar.OnScrollBarChange)global::System.Delegate.Combine((global::System.Delegate)uIScrollBar.onChange, (global::System.Delegate)new UIScrollBar.OnScrollBarChange(OnHorizontalBar));
			horizontalScrollBar.alpha = ((showScrollBars != ShowCondition.Always && !shouldMoveHorizontally) ? 0f : 1f);
		}
		if ((Object)(object)verticalScrollBar != (Object)null)
		{
			UIScrollBar uIScrollBar2 = verticalScrollBar;
			uIScrollBar2.onChange = (UIScrollBar.OnScrollBarChange)global::System.Delegate.Combine((global::System.Delegate)uIScrollBar2.onChange, (global::System.Delegate)new UIScrollBar.OnScrollBarChange(OnVerticalBar));
			verticalScrollBar.alpha = ((showScrollBars != ShowCondition.Always && !shouldMoveVertically) ? 0f : 1f);
		}
	}

	public bool RestrictWithinBounds(bool instant)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = mPanel;
		Bounds val = bounds;
		Vector2 min = (Vector2)(val.min);
		Bounds val2 = bounds;
		Vector3 val3 = uIPanel.CalculateConstrainOffset(min, (Vector2)(val2.max));
		if (val3.magnitude > 0.001f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(((Component)mPanel).gameObject, mTrans.localPosition + val3, 13f);
			}
			else
			{
				MoveRelative(val3);
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	public void DisableSpring()
	{
		SpringPanel component = ((Component)this).GetComponent<SpringPanel>();
		if ((Object)(object)component != (Object)null)
		{
			((Behaviour)component).enabled = false;
		}
	}

	public void UpdateScrollbars(bool recalculateBounds)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mPanel == (Object)null)
		{
			return;
		}
		if ((Object)(object)horizontalScrollBar != (Object)null || (Object)(object)verticalScrollBar != (Object)null)
		{
			if (recalculateBounds)
			{
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
			}
			Bounds val = bounds;
			Vector2 val2 = (Vector2)(val.min);
			Vector2 val3 = (Vector2)(val.max);
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				Vector2 clipSoftness = mPanel.clipSoftness;
				val2 -= clipSoftness;
				val3 += clipSoftness;
			}
			if ((Object)(object)horizontalScrollBar != (Object)null && val3.x > val2.x)
			{
				Vector4 clipRange = mPanel.clipRange;
				float num = clipRange.z * 0.5f;
				float num2 = clipRange.x - num - val.min.x;
				float num3 = val.max.x - num - clipRange.x;
				float num4 = val3.x - val2.x;
				num2 = Mathf.Clamp01(num2 / num4);
				num3 = Mathf.Clamp01(num3 / num4);
				float num5 = num2 + num3;
				mIgnoreCallbacks = true;
				horizontalScrollBar.barSize = 1f - num5;
				horizontalScrollBar.scrollValue = ((!(num5 > 0.001f)) ? 0f : (num2 / num5));
				mIgnoreCallbacks = false;
			}
			if ((Object)(object)verticalScrollBar != (Object)null && val3.y > val2.y)
			{
				Vector4 clipRange2 = mPanel.clipRange;
				float num6 = clipRange2.w * 0.5f;
				float num7 = clipRange2.y - num6 - val2.y;
				float num8 = val3.y - num6 - clipRange2.y;
				float num9 = val3.y - val2.y;
				num7 = Mathf.Clamp01(num7 / num9);
				num8 = Mathf.Clamp01(num8 / num9);
				float num10 = num7 + num8;
				mIgnoreCallbacks = true;
				verticalScrollBar.barSize = 1f - num10;
				verticalScrollBar.scrollValue = ((!(num10 > 0.001f)) ? 0f : (1f - num7 / num10));
				mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			mCalculatedBounds = false;
		}
	}

	public void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		DisableSpring();
		Bounds val = bounds;
		if (val.min.x == val.max.x || val.min.y == val.max.x)
		{
			return;
		}
		Vector4 clipRange = mPanel.clipRange;
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		float num3 = val.min.x + num;
		float num4 = val.max.x - num;
		float num5 = val.min.y + num2;
		float num6 = val.max.y - num2;
		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= mPanel.clipSoftness.x;
			num4 += mPanel.clipSoftness.x;
			num5 -= mPanel.clipSoftness.y;
			num6 += mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = mTrans.localPosition;
			if (scale.x != 0f)
			{
				localPosition.x += clipRange.x - num7;
			}
			if (scale.y != 0f)
			{
				localPosition.y += clipRange.y - num8;
			}
			mTrans.localPosition = localPosition;
		}
		clipRange.x = num7;
		clipRange.y = num8;
		mPanel.clipRange = clipRange;
		if (updateScrollbars)
		{
			UpdateScrollbars(false);
		}
	}

	public void ResetPosition()
	{
		mCalculatedBounds = false;
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, false);
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, true);
	}

	private void OnHorizontalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!((Object)(object)horizontalScrollBar != (Object)null)) ? 0f : horizontalScrollBar.scrollValue);
			float y = ((!((Object)(object)verticalScrollBar != (Object)null)) ? 0f : verticalScrollBar.scrollValue);
			SetDragAmount(x, y, false);
		}
	}

	private void OnVerticalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!((Object)(object)horizontalScrollBar != (Object)null)) ? 0f : horizontalScrollBar.scrollValue);
			float y = ((!((Object)(object)verticalScrollBar != (Object)null)) ? 0f : verticalScrollBar.scrollValue);
			SetDragAmount(x, y, false);
		}
	}

	public void MoveRelative(Vector3 relative)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Transform obj = mTrans;
		obj.localPosition += relative;
		Vector4 clipRange = mPanel.clipRange;
		clipRange.x -= relative.x;
		clipRange.y -= relative.y;
		mPanel.clipRange = clipRange;
		UpdateScrollbars(false);
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = mTrans.InverseTransformPoint(absolute);
		Vector3 val2 = mTrans.InverseTransformPoint(Vector3.zero);
		MoveRelative(val - val2);
	}

	public void Press(bool pressed)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).enabled || !NGUITools.GetActive(((Component)this).gameObject))
		{
			return;
		}
		if (!pressed && mDragID == UICamera.currentTouchID)
		{
			mDragID = -10;
		}
		mCalculatedBounds = false;
		mShouldMove = shouldMove;
		if (!mShouldMove)
		{
			return;
		}
		mPressed = pressed;
		if (pressed)
		{
			mMomentum = Vector3.zero;
			mScroll = 0f;
			DisableSpring();
			mLastPos = UICamera.lastHit.point;
			mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);
			return;
		}
		if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None && dragEffect == DragEffect.MomentumAndSpring)
		{
			RestrictWithinBounds(false);
		}
		if (onDragFinished != null)
		{
			onDragFinished();
		}
	}

	public void Drag(Vector2 delta)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).enabled || !NGUITools.GetActive(((Component)this).gameObject) || !mShouldMove)
		{
			return;
		}
		if (mDragID == -10)
		{
			mDragID = UICamera.currentTouchID;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		Ray val = UICamera.currentCamera.ScreenPointToRay((Vector2)(UICamera.currentTouch.pos));
		float num = 0f;
		if (mPlane.Raycast(val, out num))
		{
			Vector3 point = val.GetPoint(num);
			Vector3 val2 = point - mLastPos;
			mLastPos = point;
			if (val2.x != 0f || val2.y != 0f)
			{
				val2 = mTrans.InverseTransformDirection(val2);
				val2.Scale(scale);
				val2 = mTrans.TransformDirection(val2);
			}
			mMomentum = Vector3.Lerp(mMomentum, mMomentum + val2 * (0.01f * momentumAmount), 0.67f);
			MoveAbsolute(val2);
			if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None && dragEffect != DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds(true);
			}
		}
	}

	public void Scroll(float delta)
	{
		if (((Behaviour)this).enabled && NGUITools.GetActive(((Component)this).gameObject) && scrollWheelFactor != 0f)
		{
			DisableSpring();
			mShouldMove = shouldMove;
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		if (mPanel.changedLastFrame)
		{
			UpdateScrollbars(true);
		}
		if (repositionClipping)
		{
			repositionClipping = false;
			mCalculatedBounds = false;
			SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		float num = UpdateRealTimeDelta();
		if (showScrollBars != ShowCondition.Always)
		{
			bool flag = false;
			bool flag2 = false;
			if (showScrollBars != ShowCondition.WhenDragging || mDragID != -10)
			{
				flag = shouldMoveVertically;
				flag2 = shouldMoveHorizontally;
			}
			if (((Object)(object)verticalScrollBar != (Object)null))
			{
				float alpha = verticalScrollBar.alpha;
				alpha += ((!flag) ? ((0f - num) * 3f) : (num * 6f));
				alpha = Mathf.Clamp01(alpha);
				if (verticalScrollBar.alpha != alpha)
				{
					verticalScrollBar.alpha = alpha;
				}
			}
			if (((Object)(object)horizontalScrollBar != (Object)null))
			{
				float alpha2 = horizontalScrollBar.alpha;
				alpha2 += ((!flag2) ? ((0f - num) * 3f) : (num * 6f));
				alpha2 = Mathf.Clamp01(alpha2);
				if (horizontalScrollBar.alpha != alpha2)
				{
					horizontalScrollBar.alpha = alpha2;
				}
			}
		}
		if (mShouldMove && !mPressed)
		{
			mMomentum -= scale * (mScroll * 0.05f);
			if (mMomentum.magnitude > 0.0001f)
			{
				mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, num);
				Vector3 absolute = NGUIMath.SpringDampen(ref mMomentum, 9f, num);
				MoveAbsolute(absolute);
				if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None)
				{
					RestrictWithinBounds(false);
				}
				return;
			}
			mScroll = 0f;
			mMomentum = Vector3.zero;
		}
		else
		{
			mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, num);
	}
}
