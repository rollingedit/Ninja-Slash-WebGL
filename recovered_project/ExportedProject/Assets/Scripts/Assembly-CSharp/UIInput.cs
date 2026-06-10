using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
	public delegate char Validator(string currentText, char nextChar);

	public enum KeyboardType
	{
		Default = 0,
		ASCIICapable = 1,
		NumbersAndPunctuation = 2,
		URL = 3,
		NumberPad = 4,
		PhonePad = 5,
		NamePhonePad = 6,
		EmailAddress = 7
	}

	public delegate void OnSubmit(string inputString);

	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public string caratChar = "|";

	public Validator validator;

	public KeyboardType type;

	public bool isPassword;

	public Color activeColor = Color.white;

	public GameObject eventReceiver;

	public string functionName = "OnSubmit";

	public OnSubmit onSubmit;

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private UIWidget.Pivot mPivot = UIWidget.Pivot.Left;

	private float mPosition;

	private string mLastIME = string.Empty;

	private bool mDoInit = true;

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			if (mDoInit)
			{
				Init();
			}
			mText = value;
			if ((Object)(object)label != (Object)null)
			{
				if (string.IsNullOrEmpty(value))
				{
					value = mDefaultText;
				}
				label.supportEncoding = false;
				label.text = ((!selected) ? value : (value + caratChar));
				label.showLastPasswordChar = selected;
				label.color = ((!selected && !(value != mDefaultText)) ? mDefaultColor : activeColor);
			}
		}
	}

	public bool selected
	{
		get
		{
			return (Object)(object)UICamera.selectedObject == (Object)(object)((Component)this).gameObject;
		}
		set
		{
			if (!value && (Object)(object)UICamera.selectedObject == (Object)(object)((Component)this).gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = ((Component)this).gameObject;
			}
		}
	}

	protected void Init()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (mDoInit)
		{
			mDoInit = false;
			if ((Object)(object)label == (Object)null)
			{
				label = ((Component)this).GetComponentInChildren<UILabel>();
			}
			if ((Object)(object)label != (Object)null)
			{
				mDefaultText = label.text;
				mDefaultColor = label.color;
				label.supportEncoding = false;
				mPivot = label.pivot;
				mPosition = label.cachedTransform.localPosition.x;
			}
			else
			{
				((Behaviour)this).enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (UICamera.IsHighlighted(((Component)this).gameObject))
		{
			OnSelect(true);
		}
	}

	private void OnDisable()
	{
		if (UICamera.IsHighlighted(((Component)this).gameObject))
		{
			OnSelect(false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (mDoInit)
		{
			Init();
		}
		if (!((Object)(object)label != (Object)null) || !((Behaviour)this).enabled || !NGUITools.GetActive(((Component)this).gameObject))
		{
			return;
		}
		if (isSelected)
		{
			mText = ((!(label.text == mDefaultText)) ? label.text : string.Empty);
			label.color = activeColor;
			if (isPassword)
			{
				label.password = true;
			}
			Input.imeCompositionMode = (IMECompositionMode)1;
			Transform cachedTransform = label.cachedTransform;
			Vector3 val = (Vector2)(label.pivotOffset);
			val.y += label.relativeSize.y;
			val = cachedTransform.TransformPoint(val);
			Input.compositionCursorPos = (Vector2)(UICamera.currentCamera.WorldToScreenPoint(val));
			UpdateLabel();
			return;
		}
		if (string.IsNullOrEmpty(mText))
		{
			label.text = mDefaultText;
			label.color = mDefaultColor;
			if (isPassword)
			{
				label.password = false;
			}
		}
		else
		{
			label.text = mText;
		}
		label.showLastPasswordChar = false;
		Input.imeCompositionMode = (IMECompositionMode)2;
		RestoreLabel();
	}

	private void Update()
	{
		if (selected && mLastIME != Input.compositionString)
		{
			mLastIME = Input.compositionString;
			UpdateLabel();
		}
	}

	private void OnInput(string input)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Invalid comparison between Unknown and I4
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		if (mDoInit)
		{
			Init();
		}
		if (!selected || !((Behaviour)this).enabled || !NGUITools.GetActive(((Component)this).gameObject) || (int)Application.platform == 11 || (int)Application.platform == 8)
		{
			return;
		}
		int i = 0;
		for (int length = input.Length; i < length; i++)
		{
			char c = input[i];
			if (c == '\b')
			{
				if (mText.Length > 0)
				{
					mText = mText.Substring(0, mText.Length - 1);
					((Component)this).SendMessage("OnInputChanged", (object)this, (SendMessageOptions)1);
				}
			}
			else if (c == '\r' || c == '\n')
			{
				if (((int)UICamera.current.submitKey0 == 13 || (int)UICamera.current.submitKey1 == 13) && (!label.multiLine || (!Input.GetKey((KeyCode)306) && !Input.GetKey((KeyCode)305))))
				{
					current = this;
					if (onSubmit != null)
					{
						onSubmit(mText);
					}
					if ((Object)(object)eventReceiver == (Object)null)
					{
						eventReceiver = ((Component)this).gameObject;
					}
					eventReceiver.SendMessage(functionName, (object)mText, (SendMessageOptions)1);
					current = null;
					selected = false;
					return;
				}
				if (validator != null)
				{
					c = validator(mText, c);
				}
				if (c == '\0')
				{
					continue;
				}
				if (c == '\n' || c == '\r')
				{
					if (label.multiLine)
					{
						mText += "\n";
					}
				}
				else
				{
					mText += c;
				}
				((Component)this).SendMessage("OnInputChanged", (object)this, (SendMessageOptions)1);
			}
			else if (c >= ' ')
			{
				if (validator != null)
				{
					c = validator(mText, c);
				}
				if (c != 0)
				{
					mText += c;
					((Component)this).SendMessage("OnInputChanged", (object)this, (SendMessageOptions)1);
				}
			}
		}
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		if (mDoInit)
		{
			Init();
		}
		if (maxChars > 0 && mText.Length > maxChars)
		{
			mText = mText.Substring(0, maxChars);
		}
		if (!((Object)(object)label.font != (Object)null))
		{
			return;
		}
		string text = ((!selected) ? mText : (mText + Input.compositionString + caratChar));
		label.supportEncoding = false;
		if (label.multiLine)
		{
			text = label.font.WrapText(text, (float)label.lineWidth / label.cachedTransform.localScale.x, 0, false, UIFont.SymbolStyle.None);
		}
		else
		{
			string endOfLineThatFits = label.font.GetEndOfLineThatFits(text, (float)label.lineWidth / label.cachedTransform.localScale.x, false, UIFont.SymbolStyle.None);
			if (endOfLineThatFits != text)
			{
				text = endOfLineThatFits;
				Vector3 localPosition = label.cachedTransform.localPosition;
				localPosition.x = mPosition + (float)label.lineWidth;
				label.cachedTransform.localPosition = localPosition;
				if (mPivot == UIWidget.Pivot.Left)
				{
					label.pivot = UIWidget.Pivot.Right;
				}
				else if (mPivot == UIWidget.Pivot.TopLeft)
				{
					label.pivot = UIWidget.Pivot.TopRight;
				}
				else if (mPivot == UIWidget.Pivot.BottomLeft)
				{
					label.pivot = UIWidget.Pivot.BottomLeft;
				}
			}
			else
			{
				RestoreLabel();
			}
		}
		label.text = text;
		label.showLastPasswordChar = selected;
	}

	private void RestoreLabel()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)label != (Object)null)
		{
			label.pivot = mPivot;
			Vector3 localPosition = label.cachedTransform.localPosition;
			localPosition.x = mPosition;
			label.cachedTransform.localPosition = localPosition;
		}
	}
}
