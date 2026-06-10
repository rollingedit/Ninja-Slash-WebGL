using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Slider")]
[ExecuteInEditMode]
public class UISlider : IgnoreTimeScale
{
	public enum Direction
	{
		Horizontal = 0,
		Vertical = 1
	}

	public delegate void OnValueChange(float val);

	public static UISlider current;

	public Transform foreground;

	public Transform thumb;

	public Direction direction;

	public Vector2 fullSize = Vector2.zero;

	public GameObject eventReceiver;

	public string functionName = "OnSliderChange";

	public OnValueChange onValueChange;

	public int numberOfSteps;

	[SerializeField]
	[HideInInspector]
	private float rawValue = 1f;

	private float mStepValue = 1f;

	private BoxCollider mCol;

	private Transform mTrans;

	private Transform mFGTrans;

	private UIWidget mFGWidget;

	private UIFilledSprite mFGFilled;

	private bool mInitDone;

	public float sliderValue
	{
		get
		{
			return mStepValue;
		}
		set
		{
			Set(value, false);
		}
	}

	private void Init()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		mInitDone = true;
		if ((Object)(object)foreground != (Object)null)
		{
			mFGWidget = ((Component)foreground).GetComponent<UIWidget>();
			mFGFilled = ((!((Object)(object)mFGWidget != (Object)null)) ? null : (mFGWidget as UIFilledSprite));
			mFGTrans = ((Component)foreground).transform;
			if (fullSize == Vector2.zero)
			{
				fullSize = (Vector2)(foreground.localScale);
			}
		}
		else if ((Object)(object)mCol != (Object)null)
		{
			if (fullSize == Vector2.zero)
			{
				fullSize = (Vector2)(mCol.size);
			}
		}
		else
		{
			Debug.LogWarning((object)"UISlider expected to find a foreground object or a box collider to work with", (Object)(object)this);
		}
	}

	private void Awake()
	{
		mTrans = ((Component)this).transform;
		Collider collider = GetComponent<Collider>();
		mCol = (BoxCollider)(object)((collider is BoxCollider) ? collider : null);
	}

	private void Start()
	{
		Init();
		if (Application.isPlaying && (Object)(object)thumb != (Object)null && (Object)(object)((Component)thumb).GetComponent<Collider>() != (Object)null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(((Component)thumb).gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)global::System.Delegate.Combine((global::System.Delegate)uIEventListener.onPress, (global::System.Delegate)new UIEventListener.BoolDelegate(OnPressThumb));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)global::System.Delegate.Combine((global::System.Delegate)uIEventListener.onDrag, (global::System.Delegate)new UIEventListener.VectorDelegate(OnDragThumb));
		}
		Set(rawValue, true);
	}

	private void OnPress(bool pressed)
	{
		if (pressed && UICamera.currentTouchID != -100)
		{
			UpdateDrag();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		float num = ((!((float)numberOfSteps > 1f)) ? 0.125f : (1f / (float)(numberOfSteps - 1)));
		if (direction == Direction.Horizontal)
		{
			if ((int)key == 276)
			{
				Set(rawValue - num, false);
			}
			else if ((int)key == 275)
			{
				Set(rawValue + num, false);
			}
		}
		else if ((int)key == 274)
		{
			Set(rawValue - num, false);
		}
		else if ((int)key == 273)
		{
			Set(rawValue + num, false);
		}
	}

	private void UpdateDrag()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)mCol == (Object)null) && !((Object)(object)UICamera.currentCamera == (Object)null) && UICamera.currentTouch != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			Ray val = UICamera.currentCamera.ScreenPointToRay((Vector2)(UICamera.currentTouch.pos));
			Plane val2 = default(Plane);
			val2 = new Plane(mTrans.rotation * Vector3.back, mTrans.position);
			float num = default(float);
			if (val2.Raycast(val, out num))
			{
				Vector3 val3 = mTrans.localPosition + mCol.center - mCol.extents;
				Vector3 val4 = mTrans.localPosition - val3;
				Vector3 val5 = mTrans.InverseTransformPoint(val.GetPoint(num));
				Vector3 val6 = val5 + val4;
				Set((direction != Direction.Horizontal) ? (val6.y / mCol.size.y) : (val6.x / mCol.size.x), false);
			}
		}
	}

	private void Set(float input, bool force)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		if (!mInitDone)
		{
			Init();
		}
		float num = Mathf.Clamp01(input);
		if (num < 0.001f)
		{
			num = 0f;
		}
		rawValue = num;
		if (numberOfSteps > 1)
		{
			num = Mathf.Round(num * (float)(numberOfSteps - 1)) / (float)(numberOfSteps - 1);
		}
		if (!force && mStepValue == num)
		{
			return;
		}
		mStepValue = num;
		Vector3 localScale = (Vector2)(fullSize);
		if (direction == Direction.Horizontal)
		{
			localScale.x *= mStepValue;
		}
		else
		{
			localScale.y *= mStepValue;
		}
		if ((Object)(object)mFGFilled != (Object)null)
		{
			mFGFilled.fillAmount = mStepValue;
		}
		else if ((Object)(object)foreground != (Object)null)
		{
			mFGTrans.localScale = localScale;
			if ((Object)(object)mFGWidget != (Object)null)
			{
				if (num > 0.001f)
				{
					((Behaviour)mFGWidget).enabled = true;
					mFGWidget.MarkAsChanged();
				}
				else
				{
					((Behaviour)mFGWidget).enabled = false;
				}
			}
		}
		if ((Object)(object)thumb != (Object)null)
		{
			Vector3 localPosition = thumb.localPosition;
			if ((Object)(object)mFGFilled != (Object)null)
			{
				if (mFGFilled.fillDirection == UIFilledSprite.FillDirection.Horizontal)
				{
					localPosition.x = ((!mFGFilled.invert) ? localScale.x : (fullSize.x - localScale.x));
				}
				else if (mFGFilled.fillDirection == UIFilledSprite.FillDirection.Vertical)
				{
					localPosition.y = ((!mFGFilled.invert) ? localScale.y : (fullSize.y - localScale.y));
				}
				else
				{
					Debug.LogWarning((object)"Slider thumb is only supported with Horizontal or Vertical fill direction", (Object)(object)this);
				}
			}
			else if (direction == Direction.Horizontal)
			{
				localPosition.x = localScale.x;
			}
			else
			{
				localPosition.y = localScale.y;
			}
			thumb.localPosition = localPosition;
		}
		if ((Object)(object)eventReceiver != (Object)null && !string.IsNullOrEmpty(functionName) && Application.isPlaying)
		{
			current = this;
			eventReceiver.SendMessage(functionName, (object)mStepValue, (SendMessageOptions)1);
			current = null;
		}
		if (onValueChange != null)
		{
			onValueChange(mStepValue);
		}
	}

	public void ForceUpdate()
	{
		Set(rawValue, true);
	}
}
