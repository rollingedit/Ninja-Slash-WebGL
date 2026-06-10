using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
	public enum Style
	{
		None = 0,
		Horizontal = 1,
		Vertical = 2,
		Both = 3,
		BasedOnHeight = 4
	}

	public Camera uiCamera;

	public UIWidget widgetContainer;

	public UIPanel panelContainer;

	public Style style;

	public Vector2 relativeSize = Vector2.one;

	private Transform mTrans;

	private UIRoot mRoot;

	private Animation mAnim;

	private void Awake()
	{
		mAnim = GetComponent<Animation>();
	}

	private void Start()
	{
		if ((Object)(object)uiCamera == (Object)null)
		{
			uiCamera = NGUITools.FindCameraForLayer(((Component)this).gameObject.layer);
		}
		mRoot = NGUITools.FindInParents<UIRoot>(((Component)this).gameObject);
	}

	private void Update()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)mAnim != (Object)null && mAnim.isPlaying) || style == Style.None)
		{
			return;
		}
		if ((Object)(object)mTrans == (Object)null)
		{
			mTrans = ((Component)this).transform;
		}
		Rect val = default(Rect);
		if ((Object)(object)panelContainer != (Object)null)
		{
			if (panelContainer.clipping == UIDrawCall.Clipping.None)
			{
				val.xMin = (float)(-Screen.width) * 0.5f;
				val.yMin = (float)(-Screen.height) * 0.5f;
				val.xMax = 0f - val.xMin;
				val.yMax = 0f - val.yMin;
			}
			else
			{
				Vector4 clipRange = panelContainer.clipRange;
				val.x = clipRange.x - clipRange.z * 0.5f;
				val.y = clipRange.y - clipRange.w * 0.5f;
				val.width = clipRange.z;
				val.height = clipRange.w;
			}
		}
		else if ((Object)(object)widgetContainer != (Object)null)
		{
			Transform cachedTransform = widgetContainer.cachedTransform;
			Vector3 localScale = cachedTransform.localScale;
			Vector3 localPosition = cachedTransform.localPosition;
			Vector3 val2 = (Vector2)(widgetContainer.relativeSize);
			Vector3 val3 = (Vector2)(widgetContainer.pivotOffset);
			val3.y -= 1f;
			val3.x *= widgetContainer.relativeSize.x * localScale.x;
			val3.y *= widgetContainer.relativeSize.y * localScale.y;
			val.x = localPosition.x + val3.x;
			val.y = localPosition.y + val3.y;
			val.width = val2.x * localScale.x;
			val.height = val2.y * localScale.y;
		}
		else
		{
			if (!((Object)(object)uiCamera != (Object)null))
			{
				return;
			}
			val = uiCamera.pixelRect;
		}
		float num = val.width;
		float num2 = val.height;
		float num3 = ((!((Object)(object)mRoot != (Object)null)) ? 1f : mRoot.pixelSizeAdjustment);
		if (num3 != 1f && num2 > 1f)
		{
			float num4 = (float)mRoot.activeHeight / num2;
			num *= num4;
			num2 *= num4;
		}
		Vector3 localScale2 = mTrans.localScale;
		if (style == Style.BasedOnHeight)
		{
			localScale2.x = relativeSize.x * num2;
			localScale2.y = relativeSize.y * num2;
		}
		else
		{
			if (style == Style.Both || style == Style.Horizontal)
			{
				localScale2.x = relativeSize.x * num;
			}
			if (style == Style.Both || style == Style.Vertical)
			{
				localScale2.y = relativeSize.y * num2;
			}
		}
		if (mTrans.localScale != localScale2)
		{
			mTrans.localScale = localScale2;
		}
	}
}
