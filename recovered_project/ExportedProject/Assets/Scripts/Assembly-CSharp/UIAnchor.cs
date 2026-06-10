using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Anchor")]
public class UIAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft = 0,
		Left = 1,
		TopLeft = 2,
		Top = 3,
		TopRight = 4,
		Right = 5,
		BottomRight = 6,
		Bottom = 7,
		Center = 8
	}

	private bool mIsWindows;

	public Camera uiCamera;

	public UIWidget widgetContainer;

	public UIPanel panelContainer;

	public Side side = Side.Center;

	public bool halfPixelOffset = true;

	public float depthOffset;

	public Vector2 relativeOffset = Vector2.zero;

	private Animation mAnim;

	private UIRoot mRoot;

	private void Awake()
	{
		mAnim = GetComponent<Animation>();
	}

	private void Start()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		mRoot = NGUITools.FindInParents<UIRoot>(((Component)this).gameObject);
		mIsWindows = (int)Application.platform == 2 || (int)Application.platform == 5 || (int)Application.platform == 7;
		if ((Object)(object)uiCamera == (Object)null)
		{
			uiCamera = NGUITools.FindCameraForLayer(((Component)this).gameObject.layer);
		}
	}

	private void Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mAnim != (Object)null && ((Behaviour)mAnim).enabled && mAnim.isPlaying)
		{
			return;
		}
		Rect val = default(Rect);
		bool flag = false;
		if ((Object)(object)panelContainer != (Object)null)
		{
			if (panelContainer.clipping == UIDrawCall.Clipping.None)
			{
				float num = ((!((Object)(object)mRoot != (Object)null)) ? 0.5f : ((float)mRoot.manualHeight / (float)Screen.height * 0.5f));
				val.xMin = (float)(-Screen.width) * num;
				val.yMin = (float)(-Screen.height) * num;
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
			flag = true;
			val = uiCamera.pixelRect;
		}
		float num2 = (val.xMin + val.xMax) * 0.5f;
		float num3 = (val.yMin + val.yMax) * 0.5f;
		Vector3 val4 = default(Vector3);
		val4 = new Vector3(num2, num3, depthOffset);
		if (side != Side.Center)
		{
			if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
			{
				val4.x = val.xMax;
			}
			else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
			{
				val4.x = num2;
			}
			else
			{
				val4.x = val.xMin;
			}
			if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
			{
				val4.y = val.yMax;
			}
			else if (side == Side.Left || side == Side.Center || side == Side.Right)
			{
				val4.y = num3;
			}
			else
			{
				val4.y = val.yMin;
			}
		}
		float width = val.width;
		float height = val.height;
		val4.x += relativeOffset.x * width;
		val4.y += relativeOffset.y * height;
		if (flag)
		{
			if (uiCamera.orthographic)
			{
				val4.x = Mathf.RoundToInt(val4.x);
				val4.y = Mathf.RoundToInt(val4.y);
				if (halfPixelOffset && mIsWindows)
				{
					val4.x -= 0.5f;
					val4.y += 0.5f;
				}
			}
			val4 = uiCamera.ScreenToWorldPoint(val4);
			if (((Component)this).transform.position != val4)
			{
				((Component)this).transform.position = val4;
			}
			return;
		}
		val4.x = Mathf.RoundToInt(val4.x);
		val4.y = Mathf.RoundToInt(val4.y);
		if ((Object)(object)panelContainer != (Object)null)
		{
			val4 = ((Component)panelContainer).transform.TransformPoint(val4);
		}
		else if ((Object)(object)widgetContainer != (Object)null)
		{
			Transform parent = ((Component)widgetContainer).transform.parent;
			if ((Object)(object)parent != (Object)null)
			{
				val4 = parent.TransformPoint(val4);
			}
		}
		if (((Component)this).transform.position != val4)
		{
			((Component)this).transform.position = val4;
		}
	}
}
