using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Label")]
public class UILabel : UIWidget
{
	public enum Effect
	{
		None = 0,
		Shadow = 1,
		Outline = 2
	}

	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	[SerializeField]
	[HideInInspector]
	private string mText = string.Empty;

	[SerializeField]
	[HideInInspector]
	private int mMaxLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	[SerializeField]
	[HideInInspector]
	private int mMaxLineCount;

	[HideInInspector]
	[SerializeField]
	private bool mPassword;

	[HideInInspector]
	[SerializeField]
	private bool mShowLastChar;

	[HideInInspector]
	[SerializeField]
	private Effect mEffectStyle;

	[SerializeField]
	[HideInInspector]
	private Color mEffectColor = Color.black;

	[HideInInspector]
	[SerializeField]
	private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;

	[HideInInspector]
	[SerializeField]
	private Vector2 mEffectDistance = Vector2.one;

	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	[SerializeField]
	[HideInInspector]
	private bool mMultiline = true;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private Vector3 mLastScale = Vector3.one;

	private string mLastText = string.Empty;

	private int mLastWidth;

	private bool mLastEncoding = true;

	private int mLastCount;

	private bool mLastPass;

	private bool mLastShow;

	private Effect mLastEffect;

	private Vector3 mSize = Vector3.zero;

	private bool hasChanged
	{
		get
		{
			return mShouldBeProcessed || mLastText != text || mLastWidth != mMaxLineWidth || mLastEncoding != mEncoding || mLastCount != mMaxLineCount || mLastPass != mPassword || mLastShow != mShowLastChar || mLastEffect != mEffectStyle;
		}
		set
		{
			if (value)
			{
				mChanged = true;
				mShouldBeProcessed = true;
				return;
			}
			mShouldBeProcessed = false;
			mLastText = text;
			mLastWidth = mMaxLineWidth;
			mLastEncoding = mEncoding;
			mLastCount = mMaxLineCount;
			mLastPass = mPassword;
			mLastShow = mShowLastChar;
			mLastEffect = mEffectStyle;
		}
	}

	public UIFont font
	{
		get
		{
			return mFont;
		}
		set
		{
			if ((Object)(object)mFont != (Object)(object)value)
			{
				mFont = value;
				material = ((!((Object)(object)mFont != (Object)null)) ? null : mFont.material);
				mChanged = true;
				hasChanged = true;
				MarkAsChanged();
			}
		}
	}

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			if (value != null && mText != value)
			{
				mText = value;
				hasChanged = true;
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return mEncoding;
		}
		set
		{
			if (mEncoding != value)
			{
				mEncoding = value;
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public UIFont.SymbolStyle symbolStyle
	{
		get
		{
			return mSymbols;
		}
		set
		{
			if (mSymbols != value)
			{
				mSymbols = value;
				hasChanged = true;
			}
		}
	}

	public int lineWidth
	{
		get
		{
			return mMaxLineWidth;
		}
		set
		{
			if (mMaxLineWidth != value)
			{
				mMaxLineWidth = value;
				hasChanged = true;
			}
		}
	}

	public bool multiLine
	{
		get
		{
			return mMaxLineCount != 1;
		}
		set
		{
			if (mMaxLineCount != 1 != value)
			{
				mMaxLineCount = ((!value) ? 1 : 0);
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public int maxLineCount
	{
		get
		{
			return mMaxLineCount;
		}
		set
		{
			if (mMaxLineCount != value)
			{
				mMaxLineCount = Mathf.Max(value, 0);
				hasChanged = true;
				if (value == 1)
				{
					mPassword = false;
				}
			}
		}
	}

	public bool password
	{
		get
		{
			return mPassword;
		}
		set
		{
			if (mPassword != value)
			{
				if (value)
				{
					mMaxLineCount = 1;
					mEncoding = false;
				}
				mPassword = value;
				hasChanged = true;
			}
		}
	}

	public bool showLastPasswordChar
	{
		get
		{
			return mShowLastChar;
		}
		set
		{
			if (mShowLastChar != value)
			{
				mShowLastChar = value;
				hasChanged = true;
			}
		}
	}

	public Effect effectStyle
	{
		get
		{
			return mEffectStyle;
		}
		set
		{
			if (mEffectStyle != value)
			{
				mEffectStyle = value;
				hasChanged = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mEffectColor;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (!mEffectColor.Equals((object)value))
			{
				mEffectColor = value;
				if (mEffectStyle != Effect.None)
				{
					hasChanged = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mEffectDistance;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (mEffectDistance != value)
			{
				mEffectDistance = value;
				hasChanged = true;
			}
		}
	}

	public string processedText
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (mLastScale != base.cachedTransform.localScale)
			{
				mLastScale = base.cachedTransform.localScale;
				mShouldBeProcessed = true;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mProcessedText;
		}
	}

	public override Material material
	{
		get
		{
			Material val = base.material;
			if ((Object)(object)val == (Object)null)
			{
				val = (material = ((!((Object)(object)mFont != (Object)null)) ? null : mFont.material));
			}
			return val;
		}
	}

	public override Vector2 relativeSize
	{
		get
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mFont == (Object)null)
			{
				return (Vector2)(Vector3.one);
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return (Vector2)(mSize);
		}
	}

	protected override void OnStart()
	{
		if (mLineWidth > 0f)
		{
			mMaxLineWidth = Mathf.RoundToInt(mLineWidth);
			mLineWidth = 0f;
		}
		if (!mMultiline)
		{
			mMaxLineCount = 1;
			mMultiline = true;
		}
	}

	public override void MarkAsChanged()
	{
		hasChanged = true;
		base.MarkAsChanged();
	}

	private void ProcessText()
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		mChanged = true;
		hasChanged = false;
		mLastText = mText;
		mProcessedText = mText.Replace("\\n", "\n");
		if (mPassword)
		{
			string text = string.Empty;
			if (mShowLastChar)
			{
				int i = 1;
				for (int length = mProcessedText.Length; i < length; i++)
				{
					text += "*";
				}
				if (mProcessedText.Length > 0)
				{
					text = string.Concat((object)text, (object)mProcessedText[mProcessedText.Length - 1]);
				}
			}
			else
			{
				int j = 0;
				for (int length2 = mProcessedText.Length; j < length2; j++)
				{
					text += "*";
				}
			}
			mProcessedText = mFont.WrapText(text, (float)mMaxLineWidth / base.cachedTransform.localScale.x, mMaxLineCount, false, UIFont.SymbolStyle.None);
		}
		else if (mMaxLineWidth > 0)
		{
			mProcessedText = mFont.WrapText(mProcessedText, (float)mMaxLineWidth / base.cachedTransform.localScale.x, mMaxLineCount, mEncoding, mSymbols);
		}
		else if (mMaxLineCount > 0)
		{
			mProcessedText = mFont.WrapText(mProcessedText, 100000f, mMaxLineCount, mEncoding, mSymbols);
		}
		mSize = (Vector2)(string.IsNullOrEmpty(mProcessedText) ? Vector2.one : mFont.CalculatePrintedSize(mProcessedText, mEncoding, mSymbols));
		float x = base.cachedTransform.localScale.x;
		mSize.x = Mathf.Max(mSize.x, (!((Object)(object)mFont != (Object)null) || !(x > 1f)) ? 1f : ((float)lineWidth / x));
		mSize.y = Mathf.Max(mSize.y, 1f);
	}

	public void MakePositionPerfect()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		float num = ((!((Object)(object)font.atlas != (Object)null)) ? 1f : font.atlas.pixelSize);
		Vector3 localScale = base.cachedTransform.localScale;
		if (mFont.size == Mathf.RoundToInt(localScale.x / num) && mFont.size == Mathf.RoundToInt(localScale.y / num) && base.cachedTransform.localRotation == Quaternion.identity)
		{
			Vector2 val = relativeSize * localScale.x;
			int num2 = Mathf.RoundToInt(val.x / num);
			int num3 = Mathf.RoundToInt(val.y / num);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.FloorToInt(localPosition.x / num);
			localPosition.y = Mathf.CeilToInt(localPosition.y / num);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (num2 % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
			{
				localPosition.x += 0.5f;
			}
			if (num3 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
			{
				localPosition.y -= 0.5f;
			}
			localPosition.x *= num;
			localPosition.y *= num;
			if (base.cachedTransform.localPosition != localPosition)
			{
				base.cachedTransform.localPosition = localPosition;
			}
		}
	}

	public override void MakePixelPerfect()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mFont != (Object)null)
		{
			float num = ((!((Object)(object)font.atlas != (Object)null)) ? 1f : font.atlas.pixelSize);
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = (float)mFont.size * num;
			localScale.y = localScale.x;
			localScale.z = 1f;
			Vector2 val = relativeSize * localScale.x;
			int num2 = Mathf.RoundToInt(val.x / num);
			int num3 = Mathf.RoundToInt(val.y / num);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.FloorToInt(localPosition.x / num);
			localPosition.y = Mathf.CeilToInt(localPosition.y / num);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (base.cachedTransform.localRotation == Quaternion.identity)
			{
				if (num2 % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
				{
					localPosition.x += 0.5f;
				}
				if (num3 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
				{
					localPosition.y -= 0.5f;
				}
			}
			localPosition.x *= num;
			localPosition.y *= num;
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = localScale;
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		Color val = mEffectColor;
		val.a *= base.color.a;
		Color32 val2 = (Color32)(val);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 val3 = verts.buffer[i];
			val3.x += x;
			val3.y += y;
			verts.buffer[i] = val3;
			cols.buffer[i] = val2;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mFont == (Object)null)
		{
			return;
		}
		MakePositionPerfect();
		Pivot pivot = base.pivot;
		int size = verts.size;
		switch (pivot)
		{
		case Pivot.TopLeft:
		case Pivot.Left:
		case Pivot.BottomLeft:
			mFont.Print(processedText, (Color32)(base.color), verts, uvs, cols, mEncoding, mSymbols, UIFont.Alignment.Left, 0);
			break;
		case Pivot.TopRight:
		case Pivot.Right:
		case Pivot.BottomRight:
			mFont.Print(processedText, (Color32)(base.color), verts, uvs, cols, mEncoding, mSymbols, UIFont.Alignment.Right, Mathf.RoundToInt(relativeSize.x * (float)mFont.size));
			break;
		default:
			mFont.Print(processedText, (Color32)(base.color), verts, uvs, cols, mEncoding, mSymbols, UIFont.Alignment.Center, Mathf.RoundToInt(relativeSize.x * (float)mFont.size));
			break;
		}
		if (effectStyle == Effect.None)
		{
			return;
		}
		Vector3 localScale = base.cachedTransform.localScale;
		if (localScale.x != 0f && localScale.y != 0f)
		{
			int size2 = verts.size;
			float num = 1f / (float)mFont.size;
			float num2 = num * mEffectDistance.x;
			float num3 = num * mEffectDistance.y;
			ApplyShadow(verts, uvs, cols, size, size2, num2, 0f - num3);
			if (effectStyle == Effect.Outline)
			{
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num2, num3);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, num2, num3);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num2, 0f - num3);
			}
		}
	}
}
