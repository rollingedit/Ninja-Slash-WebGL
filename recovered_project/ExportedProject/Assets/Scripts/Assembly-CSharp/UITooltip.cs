using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	private static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public UISlicedSprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	private Transform mTrans;

	private float mTarget;

	private float mCurrent;

	private Vector3 mPos;

	private Vector3 mSize;

	private UIWidget[] mWidgets;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void Start()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		mTrans = ((Component)this).transform;
		mWidgets = ((Component)this).GetComponentsInChildren<UIWidget>();
		mPos = mTrans.localPosition;
		mSize = mTrans.localScale;
		if ((Object)(object)uiCamera == (Object)null)
		{
			uiCamera = NGUITools.FindCameraForLayer(((Component)this).gameObject.layer);
		}
		SetAlpha(0f);
	}

	private void Update()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (mCurrent != mTarget)
		{
			mCurrent = Mathf.Lerp(mCurrent, mTarget, Time.deltaTime * appearSpeed);
			if (Mathf.Abs(mCurrent - mTarget) < 0.001f)
			{
				mCurrent = mTarget;
			}
			SetAlpha(mCurrent * mCurrent);
			if (scalingTransitions)
			{
				Vector3 val = mSize * 0.25f;
				val.y = 0f - val.y;
				Vector3 localScale = Vector3.one * (1.5f - mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(mPos - val, mPos, mCurrent);
				mTrans.localPosition = localPosition;
				mTrans.localScale = localScale;
			}
		}
	}

	private void SetAlpha(float val)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		int i = 0;
		for (int num = mWidgets.Length; i < num; i++)
		{
			UIWidget uIWidget = mWidgets[i];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
		}
	}

	private void SetText(string tooltipText)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)text != (Object)null && !string.IsNullOrEmpty(tooltipText))
		{
			mTarget = 1f;
			if ((Object)(object)text != (Object)null)
			{
				text.text = tooltipText;
			}
			mPos = Input.mousePosition;
			if ((Object)(object)background != (Object)null)
			{
				Transform transform = ((Component)background).transform;
				Transform transform2 = ((Component)text).transform;
				Vector3 localPosition = transform2.localPosition;
				Vector3 localScale = transform2.localScale;
				mSize = (Vector2)(text.relativeSize);
				UITooltip uITooltip = this;
				uITooltip.mSize.x = uITooltip.mSize.x * localScale.x;
				UITooltip uITooltip2 = this;
				uITooltip2.mSize.y = uITooltip2.mSize.y * localScale.y;
				UITooltip uITooltip3 = this;
				uITooltip3.mSize.x = uITooltip3.mSize.x + (background.border.x + background.border.z + (localPosition.x - background.border.x) * 2f);
				UITooltip uITooltip4 = this;
				uITooltip4.mSize.y = uITooltip4.mSize.y + (background.border.y + background.border.w + (0f - localPosition.y - background.border.y) * 2f);
				mSize.z = 1f;
				transform.localScale = mSize;
			}
			if ((Object)(object)uiCamera != (Object)null)
			{
				mPos.x = Mathf.Clamp01(mPos.x / (float)Screen.width);
				mPos.y = Mathf.Clamp01(mPos.y / (float)Screen.height);
				float num = uiCamera.orthographicSize / mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 val = default(Vector2);
				val = new Vector2(num2 * mSize.x / (float)Screen.width, num2 * mSize.y / (float)Screen.height);
				mPos.x = Mathf.Min(mPos.x, 1f - val.x);
				mPos.y = Mathf.Max(mPos.y, val.y);
				mTrans.position = uiCamera.ViewportToWorldPoint(mPos);
				mPos = mTrans.localPosition;
				mPos.x = Mathf.Round(mPos.x);
				mPos.y = Mathf.Round(mPos.y);
				mTrans.localPosition = mPos;
			}
			else
			{
				if (mPos.x + mSize.x > (float)Screen.width)
				{
					mPos.x = (float)Screen.width - mSize.x;
				}
				if (mPos.y - mSize.y < 0f)
				{
					mPos.y = mSize.y;
				}
				UITooltip uITooltip5 = this;
				uITooltip5.mPos.x = uITooltip5.mPos.x - (float)Screen.width * 0.5f;
				UITooltip uITooltip6 = this;
				uITooltip6.mPos.y = uITooltip6.mPos.y - (float)Screen.height * 0.5f;
			}
		}
		else
		{
			mTarget = 0f;
		}
	}

	public static void ShowText(string tooltipText)
	{
		if ((Object)(object)mInstance != (Object)null)
		{
			mInstance.SetText(tooltipText);
		}
	}
}
