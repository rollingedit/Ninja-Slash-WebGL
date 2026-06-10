using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : MonoBehaviour
{
	public enum Position
	{
		Auto = 0,
		Above = 1,
		Below = 2
	}

	public delegate void OnSelectionChange(string item);

	private const float animSpeed = 0.15f;

	public static UIPopupList current;

	public UIAtlas atlas;

	public UIFont font;

	public UILabel textLabel;

	public string backgroundSprite;

	public string highlightSprite;

	public Position position;

	public List<string> items = new List<string>();

	public Vector2 padding = (Vector2)(new Vector3(4f, 4f));

	public float textScale = 1f;

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.59607846f, 1f, 0.2f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public GameObject eventReceiver;

	public string functionName = "OnSelectionChange";

	public OnSelectionChange onSelectionChange;

	[HideInInspector]
	[SerializeField]
	private string mSelectedItem;

	private UIPanel mPanel;

	private GameObject mChild;

	private UISprite mBackground;

	private UISprite mHighlight;

	private UILabel mHighlightedLabel;

	private List<UILabel> mLabelList = new List<UILabel>();

	private float mBgBorder;

	public bool isOpen
	{
		get
		{
			return (Object)(object)mChild != (Object)null;
		}
	}

	public string selection
	{
		get
		{
			return mSelectedItem;
		}
		set
		{
			if (mSelectedItem != value)
			{
				mSelectedItem = value;
				if ((Object)(object)textLabel != (Object)null)
				{
					textLabel.text = ((!isLocalized || !((Object)(object)Localization.instance != (Object)null)) ? value : Localization.instance.Get(value));
				}
				current = this;
				if (onSelectionChange != null)
				{
					onSelectionChange(mSelectedItem);
				}
				if ((Object)(object)eventReceiver != (Object)null && !string.IsNullOrEmpty(functionName) && Application.isPlaying)
				{
					eventReceiver.SendMessage(functionName, (object)mSelectedItem, (SendMessageOptions)1);
				}
				current = null;
			}
		}
	}

	private bool handleEvents
	{
		get
		{
			UIButtonKeys component = ((Component)this).GetComponent<UIButtonKeys>();
			return (Object)(object)component == (Object)null || !((Behaviour)component).enabled;
		}
		set
		{
			UIButtonKeys component = ((Component)this).GetComponent<UIButtonKeys>();
			if ((Object)(object)component != (Object)null)
			{
				((Behaviour)component).enabled = !value;
			}
		}
	}

	private void Start()
	{
		if (string.IsNullOrEmpty(mSelectedItem))
		{
			if (items.Count > 0)
			{
				selection = items[0];
			}
		}
		else
		{
			string text = mSelectedItem;
			mSelectedItem = null;
			selection = text;
		}
	}

	private void OnLocalize(Localization loc)
	{
		if (isLocalized && (Object)(object)textLabel != (Object)null)
		{
			textLabel.text = loc.Get(mSelectedItem);
		}
	}

	private void Highlight(UILabel lbl, bool instant)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)mHighlight != (Object)null))
		{
			return;
		}
		TweenPosition component = ((Component)lbl).GetComponent<TweenPosition>();
		if (!((Object)(object)component != (Object)null) || !((Behaviour)component).enabled)
		{
			mHighlightedLabel = lbl;
			UIAtlas.Sprite sprite = mHighlight.sprite;
			float num = sprite.inner.xMin - sprite.outer.xMin;
			float num2 = sprite.inner.yMin - sprite.outer.yMin;
			Vector3 val = lbl.cachedTransform.localPosition + new Vector3(0f - num, num2, 0f);
			if (instant || !isAnimated)
			{
				mHighlight.cachedTransform.localPosition = val;
			}
			else
			{
				TweenPosition.Begin(((Component)mHighlight).gameObject, 0.1f, val).method = UITweener.Method.EaseOut;
			}
		}
	}

	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			Highlight(component, false);
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		Highlight(lbl, instant);
		UIEventListener component = ((Component)lbl).gameObject.GetComponent<UIEventListener>();
		selection = component.parameter as string;
		UIButtonSound[] components = ((Component)this).GetComponents<UIButtonSound>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			UIButtonSound uIButtonSound = components[i];
			if (uIButtonSound.trigger == UIButtonSound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIButtonSound.audioClip, uIButtonSound.volume, 1f);
			}
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			Select(go.GetComponent<UILabel>(), true);
		}
	}

	private void OnKey(KeyCode key)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		if (!((Behaviour)this).enabled || !NGUITools.GetActive(((Component)this).gameObject) || !handleEvents)
		{
			return;
		}
		int num = mLabelList.IndexOf(mHighlightedLabel);
		if ((int)key == 273)
		{
			if (num > 0)
			{
				Select(mLabelList[--num], false);
			}
		}
		else if ((int)key == 274)
		{
			if (num + 1 < mLabelList.Count)
			{
				Select(mLabelList[++num], false);
			}
		}
		else if ((int)key == 27)
		{
			OnSelect(false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (isSelected || !((Object)(object)mChild != (Object)null))
		{
			return;
		}
		mLabelList.Clear();
		handleEvents = false;
		if (isAnimated)
		{
			UIWidget[] componentsInChildren = mChild.GetComponentsInChildren<UIWidget>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				UIWidget uIWidget = componentsInChildren[i];
				Color color = uIWidget.color;
				color.a = 0f;
				TweenColor.Begin(((Component)uIWidget).gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
			}
			Collider[] componentsInChildren2 = mChild.GetComponentsInChildren<Collider>();
			int j = 0;
			for (int num2 = componentsInChildren2.Length; j < num2; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			UpdateManager.AddDestroy((Object)(object)mChild, 0.15f);
		}
		else
		{
			Object.Destroy((Object)(object)mChild);
		}
		mBackground = null;
		mHighlight = null;
		mChild = null;
	}

	private void AnimateColor(UIWidget widget)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(((Component)widget).gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = ((!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z));
		widget.cachedTransform.localPosition = localPosition2;
		GameObject gameObject = ((Component)widget).gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		GameObject gameObject = ((Component)widget).gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)font.size * textScale + mBgBorder * 2f;
		Vector3 localScale = cachedTransform.localScale;
		cachedTransform.localScale = new Vector3(localScale.x, num, localScale.z);
		TweenScale.Begin(gameObject, 0.15f, localScale).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - localScale.y + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		AnimateColor(widget);
		AnimatePosition(widget, placeAbove, bottom);
	}

	private void OnClick()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mChild == (Object)null && (Object)(object)atlas != (Object)null && (Object)(object)font != (Object)null && items.Count > 1)
		{
			mLabelList.Clear();
			handleEvents = true;
			if ((Object)(object)mPanel == (Object)null)
			{
				mPanel = UIPanel.Find(((Component)this).transform, true);
			}
			Transform transform = ((Component)this).transform;
			Bounds val = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
			mChild = new GameObject("Drop-down List");
			mChild.layer = ((Component)this).gameObject.layer;
			Transform transform2 = mChild.transform;
			transform2.parent = transform.parent;
			transform2.localPosition = val.min;
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			mBackground = NGUITools.AddSprite(mChild, atlas, backgroundSprite);
			mBackground.pivot = UIWidget.Pivot.TopLeft;
			mBackground.depth = NGUITools.CalculateNextDepth(((Component)mPanel).gameObject);
			mBackground.color = backgroundColor;
			Vector4 border = mBackground.border;
			mBgBorder = border.y;
			mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
			mHighlight = NGUITools.AddSprite(mChild, atlas, highlightSprite);
			mHighlight.pivot = UIWidget.Pivot.TopLeft;
			mHighlight.color = highlightColor;
			UIAtlas.Sprite sprite = mHighlight.sprite;
			float num = sprite.inner.yMin - sprite.outer.yMin;
			float num2 = (float)font.size * textScale;
			float num3 = 0f;
			float num4 = 0f - padding.y;
			List<UILabel> val2 = new List<UILabel>();
			int i = 0;
			for (int count = items.Count; i < count; i++)
			{
				string text = items[i];
				UILabel uILabel = NGUITools.AddWidget<UILabel>(mChild);
				uILabel.pivot = UIWidget.Pivot.TopLeft;
				uILabel.font = font;
				uILabel.text = ((!isLocalized || !((Object)(object)Localization.instance != (Object)null)) ? text : Localization.instance.Get(text));
				uILabel.color = textColor;
				uILabel.cachedTransform.localPosition = new Vector3(border.x + padding.x, num4, 0f);
				uILabel.MakePixelPerfect();
				if (textScale != 1f)
				{
					Vector3 localScale = uILabel.cachedTransform.localScale;
					uILabel.cachedTransform.localScale = localScale * textScale;
				}
				val2.Add(uILabel);
				num4 -= num2;
				num4 -= padding.y;
				num3 = Mathf.Max(num3, uILabel.relativeSize.x * num2);
				UIEventListener uIEventListener = UIEventListener.Get(((Component)uILabel).gameObject);
				uIEventListener.onHover = OnItemHover;
				uIEventListener.onPress = OnItemPress;
				uIEventListener.parameter = text;
				if (mSelectedItem == text)
				{
					Highlight(uILabel, true);
				}
				mLabelList.Add(uILabel);
			}
			num3 = Mathf.Max(num3, val.size.x - (border.x + padding.x) * 2f);
			Vector3 center = default(Vector3);
			center = new Vector3(num3 * 0.5f / num2, -0.5f, 0f);
			Vector3 size = default(Vector3);
			size = new Vector3(num3 / num2, (num2 + padding.y) / num2, 1f);
			int j = 0;
			for (int count2 = val2.Count; j < count2; j++)
			{
				UILabel uILabel2 = val2[j];
				BoxCollider val3 = NGUITools.AddWidgetCollider(((Component)uILabel2).gameObject);
				center.z = val3.center.z;
				val3.center = center;
				val3.size = size;
			}
			num3 += (border.x + padding.x) * 2f;
			num4 -= border.y;
			mBackground.cachedTransform.localScale = new Vector3(num3, 0f - num4 + border.y, 1f);
			mHighlight.cachedTransform.localScale = new Vector3(num3 - (border.x + padding.x) * 2f + (sprite.inner.xMin - sprite.outer.xMin) * 2f, num2 + num * 2f, 1f);
			bool flag = position == Position.Above;
			if (position == Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(((Component)this).gameObject.layer);
				if ((Object)(object)uICamera != (Object)null)
				{
					flag = uICamera.cachedCamera.WorldToViewportPoint(transform.position).y < 0.5f;
				}
			}
			if (isAnimated)
			{
				float bottom = num4 + num2;
				Animate(mHighlight, flag, bottom);
				int k = 0;
				for (int count3 = val2.Count; k < count3; k++)
				{
					Animate(val2[k], flag, bottom);
				}
				AnimateColor(mBackground);
				AnimateScale(mBackground, flag, bottom);
			}
			if (flag)
			{
				transform2.localPosition = new Vector3(val.min.x, val.max.y - num4 - border.y, val.min.z);
			}
		}
		else
		{
			OnSelect(false);
		}
	}
}
