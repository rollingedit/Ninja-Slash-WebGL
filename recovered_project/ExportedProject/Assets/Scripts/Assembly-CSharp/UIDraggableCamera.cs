using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
[RequireComponent(typeof(Camera))]
public class UIDraggableCamera : IgnoreTimeScale
{
	public Transform rootForBounds;

	public Vector2 scale = Vector2.one;

	public float scrollWheelFactor;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Camera mCam;

	private Transform mTrans;

	private bool mPressed;

	private Vector2 mMomentum = Vector2.zero;

	private Bounds mBounds;

	private float mScroll;

	private UIRoot mRoot;

	public Vector2 currentMomentum
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
		}
	}

	private void Awake()
	{
		mCam = GetComponent<Camera>();
		mTrans = ((Component)this).transform;
		if ((Object)(object)rootForBounds == (Object)null)
		{
			Debug.LogError((object)(NGUITools.GetHierarchy(((Component)this).gameObject) + " needs the 'Root For Bounds' parameter to be set"), (Object)(object)this);
			((Behaviour)this).enabled = false;
		}
	}

	private void Start()
	{
		mRoot = NGUITools.FindInParents<UIRoot>(((Component)this).gameObject);
	}

	private Vector3 CalculateConstrainOffset()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)rootForBounds == (Object)null || rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Rect rect = mCam.rect;
		float num = rect.xMin * (float)Screen.width;
		Rect rect2 = mCam.rect;
		Vector3 val = default(Vector3);
		val = new Vector3(num, rect2.yMin * (float)Screen.height, 0f);
		Rect rect3 = mCam.rect;
		float num2 = rect3.xMax * (float)Screen.width;
		Rect rect4 = mCam.rect;
		Vector3 val2 = default(Vector3);
		val2 = new Vector3(num2, rect4.yMax * (float)Screen.height, 0f);
		val = mCam.ScreenToWorldPoint(val);
		val2 = mCam.ScreenToWorldPoint(val2);
		Vector2 minRect = default(Vector2);
		minRect = new Vector2(mBounds.min.x, mBounds.min.y);
		Vector2 maxRect = default(Vector2);
		maxRect = new Vector2(mBounds.max.x, mBounds.max.y);
		return (Vector2)(NGUIMath.ConstrainRect(minRect, maxRect, (Vector2)(val), (Vector2)(val2)));
	}

	public bool ConstrainToBounds(bool immediate)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mTrans != (Object)null && (Object)(object)rootForBounds != (Object)null)
		{
			Vector3 val = CalculateConstrainOffset();
			if (val.magnitude > 0f)
			{
				if (immediate)
				{
					Transform obj = mTrans;
					obj.position -= val;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(((Component)this).gameObject, mTrans.position - val, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	public void Press(bool isPressed)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)rootForBounds != (Object)null))
		{
			return;
		}
		mPressed = isPressed;
		if (isPressed)
		{
			mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(rootForBounds);
			mMomentum = Vector2.zero;
			mScroll = 0f;
			SpringPosition component = ((Component)this).GetComponent<SpringPosition>();
			if ((Object)(object)component != (Object)null)
			{
				((Behaviour)component).enabled = false;
			}
		}
		else if (dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
		{
			ConstrainToBounds(false);
		}
	}

	public void Drag(Vector2 delta)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if ((Object)(object)mRoot != (Object)null)
		{
			delta *= mRoot.pixelSizeAdjustment;
		}
		Vector2 val = Vector2.Scale(delta, -scale);
		Transform obj = mTrans;
		obj.localPosition += (Vector3)val;
		mMomentum = Vector2.Lerp(mMomentum, mMomentum + val * (0.01f * momentumAmount), 0.67f);
		if (dragEffect != UIDragObject.DragEffect.MomentumAndSpring && ConstrainToBounds(true))
		{
			mMomentum = Vector2.zero;
			mScroll = 0f;
		}
	}

	public void Scroll(float delta)
	{
		if (((Behaviour)this).enabled && NGUITools.GetActive(((Component)this).gameObject))
		{
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}

	private void Update()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		float deltaTime = UpdateRealTimeDelta();
		if (mPressed)
		{
			SpringPosition component = ((Component)this).GetComponent<SpringPosition>();
			if ((Object)(object)component != (Object)null)
			{
				((Behaviour)component).enabled = false;
			}
			mScroll = 0f;
		}
		else
		{
			mMomentum += scale * (mScroll * 20f);
			mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, deltaTime);
			if (mMomentum.magnitude > 0.01f)
			{
				Transform obj = mTrans;
				obj.localPosition += (Vector3)NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
				mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(rootForBounds);
				if (!ConstrainToBounds(dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component2 = ((Component)this).GetComponent<SpringPosition>();
					if ((Object)(object)component2 != (Object)null)
					{
						((Behaviour)component2).enabled = false;
					}
				}
				return;
			}
			mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
	}
}
