using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Font")]
public class UIFont : MonoBehaviour
{
	public enum SymbolStyle
	{
		None = 0,
		Uncolored = 1,
		Colored = 2
	}

	public enum Alignment
	{
		Left = 0,
		Center = 1,
		Right = 2
	}

	[SerializeField]
	[HideInInspector]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector]
	[SerializeField]
	private BMFont mFont = new BMFont();

	[SerializeField]
	[HideInInspector]
	private int mSpacingX;

	[HideInInspector]
	[SerializeField]
	private int mSpacingY;

	[SerializeField]
	[HideInInspector]
	private UIAtlas mAtlas;

	[SerializeField]
	[HideInInspector]
	private UIFont mReplacement;

	private UIAtlas.Sprite mSprite;

	private bool mSpriteSet;

	private List<Color> mColors = new List<Color>();

	public BMFont bmFont
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mFont : mReplacement.bmFont;
		}
	}

	public int texWidth
	{
		get
		{
			return ((Object)(object)mReplacement != (Object)null) ? mReplacement.texWidth : ((mFont == null) ? 1 : mFont.texWidth);
		}
	}

	public int texHeight
	{
		get
		{
			return ((Object)(object)mReplacement != (Object)null) ? mReplacement.texHeight : ((mFont == null) ? 1 : mFont.texHeight);
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mAtlas : mReplacement.atlas;
		}
		set
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.atlas = value;
			}
			else
			{
				if (!((Object)(object)mAtlas != (Object)(object)value))
				{
					return;
				}
				if ((Object)(object)value == (Object)null)
				{
					if ((Object)(object)mAtlas != (Object)null)
					{
						mMat = mAtlas.spriteMaterial;
					}
					if (sprite != null)
					{
						mUVRect = uvRect;
					}
				}
				mAtlas = value;
				MarkAsDirty();
			}
		}
	}

	public Material material
	{
		get
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				return mReplacement.material;
			}
			return (!((Object)(object)mAtlas != (Object)null)) ? mMat : mAtlas.spriteMaterial;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.material = value;
			}
			else if ((Object)(object)mAtlas == (Object)null && (Object)(object)mMat != (Object)(object)value)
			{
				mMat = value;
				MarkAsDirty();
			}
		}
	}

	public Texture2D texture
	{
		get
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				return mReplacement.texture;
			}
			Material val = material;
			object result;
			if ((Object)(object)val != (Object)null)
			{
				Texture mainTexture = val.mainTexture;
				result = ((mainTexture is Texture2D) ? mainTexture : null);
			}
			else
			{
				result = null;
			}
			return (Texture2D)result;
		}
	}

	public Rect uvRect
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mReplacement != (Object)null)
			{
				return mReplacement.uvRect;
			}
			if ((Object)(object)mAtlas != (Object)null && mSprite == null && sprite != null)
			{
				Texture val = mAtlas.texture;
				if ((Object)(object)val != (Object)null)
				{
					mUVRect = mSprite.outer;
					if (mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
					{
						mUVRect = NGUIMath.ConvertToTexCoords(mUVRect, val.width, val.height);
					}
					if (mSprite.hasPadding)
					{
						Rect val2 = mUVRect;
						mUVRect.xMin = val2.xMin - mSprite.paddingLeft * val2.width;
						mUVRect.yMin = val2.yMin - mSprite.paddingBottom * val2.height;
						mUVRect.xMax = val2.xMax + mSprite.paddingRight * val2.width;
						mUVRect.yMax = val2.yMax + mSprite.paddingTop * val2.height;
					}
					if (mSprite.hasPadding)
					{
						Trim();
					}
				}
			}
			return mUVRect;
		}
		set
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.uvRect = value;
			}
			else if (sprite == null && mUVRect != value)
			{
				mUVRect = value;
				MarkAsDirty();
			}
		}
	}

	public string spriteName
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mFont.spriteName : mReplacement.spriteName;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.spriteName = value;
			}
			else if (mFont.spriteName != value)
			{
				mFont.spriteName = value;
				MarkAsDirty();
			}
		}
	}

	public int horizontalSpacing
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mSpacingX : mReplacement.horizontalSpacing;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.horizontalSpacing = value;
			}
			else if (mSpacingX != value)
			{
				mSpacingX = value;
				MarkAsDirty();
			}
		}
	}

	public int verticalSpacing
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mSpacingY : mReplacement.verticalSpacing;
		}
		set
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				mReplacement.verticalSpacing = value;
			}
			else if (mSpacingY != value)
			{
				mSpacingY = value;
				MarkAsDirty();
			}
		}
	}

	public int size
	{
		get
		{
			return (!((Object)(object)mReplacement != (Object)null)) ? mFont.charSize : mReplacement.size;
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if ((Object)(object)mReplacement != (Object)null)
			{
				return mReplacement.sprite;
			}
			if (!mSpriteSet)
			{
				mSprite = null;
			}
			if (mSprite == null && (Object)(object)mAtlas != (Object)null && !string.IsNullOrEmpty(mFont.spriteName))
			{
				mSprite = mAtlas.GetSprite(mFont.spriteName);
				if (mSprite == null)
				{
					mSprite = mAtlas.GetSprite(((Object)this).name);
				}
				mSpriteSet = true;
				if (mSprite == null)
				{
					Debug.LogError((object)("Can't find the sprite '" + mFont.spriteName + "' in UIAtlas on " + NGUITools.GetHierarchy(((Component)mAtlas).gameObject)));
					mFont.spriteName = null;
				}
			}
			return mSprite;
		}
	}

	public UIFont replacement
	{
		get
		{
			return mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if ((Object)(object)uIFont == (Object)(object)this)
			{
				uIFont = null;
			}
			if ((Object)(object)mReplacement != (Object)(object)uIFont)
			{
				if ((Object)(object)uIFont != (Object)null && (Object)(object)uIFont.replacement == (Object)(object)this)
				{
					uIFont.replacement = null;
				}
				if ((Object)(object)mReplacement != (Object)null)
				{
					MarkAsDirty();
				}
				mReplacement = uIFont;
				MarkAsDirty();
			}
		}
	}

	private void Trim()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		Texture val = mAtlas.texture;
		if ((Object)(object)val != (Object)null && mSprite != null)
		{
			Rect val2 = NGUIMath.ConvertToPixels(mUVRect, ((Texture)texture).width, ((Texture)texture).height, true);
			Rect val3 = ((mAtlas.coordinates != UIAtlas.Coordinates.TexCoords) ? mSprite.outer : NGUIMath.ConvertToPixels(mSprite.outer, val.width, val.height, true));
			int xMin = Mathf.RoundToInt(val3.xMin - val2.xMin);
			int yMin = Mathf.RoundToInt(val3.yMin - val2.yMin);
			int xMax = Mathf.RoundToInt(val3.xMax - val2.xMin);
			int yMax = Mathf.RoundToInt(val3.yMax - val2.yMin);
			mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	private bool References(UIFont font)
	{
		if ((Object)(object)font == (Object)null)
		{
			return false;
		}
		if ((Object)(object)font == (Object)(object)this)
		{
			return true;
		}
		return (Object)(object)mReplacement != (Object)null && mReplacement.References(font);
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		if ((Object)(object)a == (Object)null || (Object)(object)b == (Object)null)
		{
			return false;
		}
		return (Object)(object)a == (Object)(object)b || a.References(b) || b.References(a);
	}

	public void MarkAsDirty()
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			mReplacement.MarkAsDirty();
			return;
		}
		mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			UILabel uILabel = array[i];
			if (((Behaviour)uILabel).enabled && NGUITools.GetActive(((Component)uILabel).gameObject) && CheckIfRelated(this, uILabel.font))
			{
				UIFont font = uILabel.font;
				uILabel.font = null;
				uILabel.font = font;
			}
		}
	}

	public Vector2 CalculatePrintedSize(string text, bool encoding, SymbolStyle symbolStyle)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
		}
		Vector2 zero = Vector2.zero;
		if (mFont != null && mFont.isValid && !string.IsNullOrEmpty(text))
		{
			if (encoding)
			{
				text = NGUITools.StripSymbols(text);
			}
			int length = text.Length;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = mFont.charSize + mSpacingY;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num2 > num)
					{
						num = num2;
					}
					num2 = 0;
					num3 += num5;
					num4 = 0;
					continue;
				}
				if (c < ' ')
				{
					num4 = 0;
					continue;
				}
				BMSymbol bMSymbol = ((!encoding || symbolStyle == SymbolStyle.None) ? null : mFont.MatchSymbol(text, i, length));
				if (bMSymbol == null)
				{
					BMGlyph glyph = mFont.GetGlyph(c);
					if (glyph != null)
					{
						num2 += mSpacingX + ((num4 == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(num4)));
						num4 = c;
					}
				}
				else
				{
					num2 += mSpacingX + bMSymbol.width;
					i += bMSymbol.length - 1;
					num4 = 0;
				}
			}
			float num6 = ((mFont.charSize <= 0) ? 1f : (1f / (float)mFont.charSize));
			zero.x = num6 * (float)((num2 <= num) ? num : num2);
			zero.y = num6 * (float)(num3 + num5);
		}
		return zero;
	}

	private static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && s[num] == ' ')
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	public string GetEndOfLineThatFits(string text, float maxWidth, bool encoding, SymbolStyle symbolStyle)
	{
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)size);
		if (num < 1)
		{
			return text;
		}
		int length = text.Length;
		int num2 = num;
		BMGlyph bMGlyph = null;
		int num3 = length;
		while (num3 > 0 && num2 > 0)
		{
			char c = text[--num3];
			BMSymbol bMSymbol = ((!encoding || symbolStyle == SymbolStyle.None) ? null : mFont.MatchSymbol(text, num3, length));
			BMGlyph bMGlyph2 = ((bMSymbol != null) ? null : mFont.GetGlyph(c));
			int num4 = mSpacingX;
			if (bMSymbol != null)
			{
				num4 += bMSymbol.width;
			}
			else
			{
				if (bMGlyph2 == null)
				{
					bMGlyph = null;
					continue;
				}
				num4 += bMGlyph2.advance + ((bMGlyph != null) ? bMGlyph.GetKerning(c) : 0);
				bMGlyph = bMGlyph2;
			}
			num2 -= num4;
		}
		if (num2 < 0)
		{
			num3++;
		}
		return text.Substring(num3, length - num3);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding, SymbolStyle symbolStyle)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mReplacement != (Object)null)
		{
			return mReplacement.WrapText(text, maxWidth, maxLineCount, encoding, symbolStyle);
		}
		int num = Mathf.RoundToInt(maxWidth * (float)size);
		if (num < 1)
		{
			return text;
		}
		StringBuilder s = new StringBuilder();
		int length = text.Length;
		int num2 = num;
		int num3 = 0;
		int i = 0;
		int j = 0;
		bool flag = true;
		bool flag2 = maxLineCount != 1;
		int num4 = 1;
		for (; j < length; j++)
		{
			char c = text[j];
			if (c == '\n')
			{
				if (!flag2 || num4 == maxLineCount)
				{
					break;
				}
				num2 = num;
				if (i < j)
				{
					s.Append(text.Substring(i, j - i + 1));
				}
				else
				{
					s.Append(c);
				}
				flag = true;
				num4++;
				i = j + 1;
				num3 = 0;
				continue;
			}
			if (c == ' ' && num3 != 32 && i < j)
			{
				s.Append(text.Substring(i, j - i + 1));
				flag = false;
				i = j + 1;
				num3 = c;
			}
			if (encoding && c == '[' && j + 2 < length)
			{
				if (text[j + 1] == '-' && text[j + 2] == ']')
				{
					j += 2;
					continue;
				}
				if (j + 7 < length && text[j + 7] == ']' && NGUITools.EncodeColor(NGUITools.ParseColor(text, j + 1)) == text.Substring(j + 1, 6).ToUpper())
				{
					j += 7;
					continue;
				}
			}
			BMSymbol bMSymbol = ((!encoding || symbolStyle == SymbolStyle.None) ? null : mFont.MatchSymbol(text, j, length));
			BMGlyph bMGlyph = ((bMSymbol != null) ? null : mFont.GetGlyph(c));
			int num5 = mSpacingX;
			if (bMSymbol != null)
			{
				num5 += bMSymbol.width;
			}
			else
			{
				if (bMGlyph == null)
				{
					continue;
				}
				num5 += ((num3 == 0) ? bMGlyph.advance : (bMGlyph.advance + bMGlyph.GetKerning(num3)));
			}
			num2 -= num5;
			if (num2 < 0)
			{
				if (!flag && flag2 && num4 != maxLineCount)
				{
					for (; i < length && text[i] == ' '; i++)
					{
					}
					flag = true;
					num2 = num;
					j = i - 1;
					num3 = 0;
					if (!flag2 || num4 == maxLineCount)
					{
						break;
					}
					num4++;
					EndLine(ref s);
					continue;
				}
				s.Append(text.Substring(i, Mathf.Max(0, j - i)));
				if (!flag2 || num4 == maxLineCount)
				{
					i = j;
					break;
				}
				EndLine(ref s);
				flag = true;
				num4++;
				if (c == ' ')
				{
					i = j + 1;
					num2 = num;
				}
				else
				{
					i = j;
					num2 = num - num5;
				}
				num3 = 0;
			}
			else
			{
				num3 = c;
			}
			if (bMSymbol != null)
			{
				j += bMSymbol.length - 1;
				num3 = 0;
			}
		}
		if (i < j)
		{
			s.Append(text.Substring(i, j - i));
		}
		return s.ToString();
	}

	public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding)
	{
		return WrapText(text, maxWidth, maxLineCount, encoding, SymbolStyle.None);
	}

	public string WrapText(string text, float maxWidth, int maxLineCount)
	{
		return WrapText(text, maxWidth, maxLineCount, false, SymbolStyle.None);
	}

	private void Align(BetterList<Vector3> verts, int indexOffset, Alignment alignment, int x, int lineWidth)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (alignment != Alignment.Left && mFont.charSize > 0)
		{
			float num = ((alignment != Alignment.Right) ? ((float)(lineWidth - x) * 0.5f) : ((float)(lineWidth - x)));
			num = Mathf.RoundToInt(num);
			if (num < 0f)
			{
				num = 0f;
			}
			num /= (float)mFont.charSize;
			for (int i = indexOffset; i < verts.size; i++)
			{
				Vector3 val = verts.buffer[i];
				val.x += num;
				verts.buffer[i] = val;
			}
		}
	}

	public void Print(string text, Color32 color, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, bool encoding, SymbolStyle symbolStyle, Alignment alignment, int lineWidth)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mReplacement != (Object)null)
		{
			mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth);
		}
		else
		{
			if (mFont == null || text == null)
			{
				return;
			}
			if (!mFont.isValid)
			{
				Debug.LogError((object)"Attempting to print using an invalid font!");
				return;
			}
			mColors.Clear();
			mColors.Add((Color32)(color));
			Vector2 val = (Vector2)((mFont.charSize <= 0) ? Vector2.one : new Vector2(1f / (float)mFont.charSize, 1f / (float)mFont.charSize));
			int num = verts.size;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = mFont.charSize + mSpacingY;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			Rect val2 = uvRect;
			float num7 = val2.width / (float)mFont.texWidth;
			float num8 = mUVRect.height / (float)mFont.texHeight;
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c == '\n')
				{
					if (num3 > num2)
					{
						num2 = num3;
					}
					if (alignment != Alignment.Left)
					{
						Align(verts, num, alignment, num3, lineWidth);
						num = verts.size;
					}
					num3 = 0;
					num4 += num6;
					num5 = 0;
					continue;
				}
				if (c < ' ')
				{
					num5 = 0;
					continue;
				}
				if (encoding && c == '[')
				{
					int num9 = NGUITools.ParseSymbol(text, i, mColors);
					if (num9 > 0)
					{
						color = (Color32)(mColors[mColors.Count - 1]);
						i += num9 - 1;
						continue;
					}
				}
				BMSymbol bMSymbol = ((!encoding || symbolStyle == SymbolStyle.None) ? null : mFont.MatchSymbol(text, i, length));
				if (bMSymbol == null)
				{
					BMGlyph glyph = mFont.GetGlyph(c);
					if (glyph == null)
					{
						continue;
					}
					if (num5 != 0)
					{
						num3 += glyph.GetKerning(num5);
					}
					if (c == ' ')
					{
						num3 += mSpacingX + glyph.advance;
						num5 = c;
						continue;
					}
					zero.x = val.x * (float)(num3 + glyph.offsetX);
					zero.y = (0f - val.y) * (float)(num4 + glyph.offsetY);
					zero2.x = zero.x + val.x * (float)glyph.width;
					zero2.y = zero.y - val.y * (float)glyph.height;
					zero3.x = mUVRect.xMin + num7 * (float)glyph.x;
					zero3.y = mUVRect.yMax - num8 * (float)glyph.y;
					zero4.x = zero3.x + num7 * (float)glyph.width;
					zero4.y = zero3.y - num8 * (float)glyph.height;
					num3 += mSpacingX + glyph.advance;
					num5 = c;
					if (glyph.channel == 0 || glyph.channel == 15)
					{
						for (int j = 0; j < 4; j++)
						{
							cols.Add(color);
						}
					}
					else
					{
						Color val3 = (Color32)(color);
						val3 *= 0.49f;
						switch (glyph.channel)
						{
						case 1:
							val3.b += 0.51f;
							break;
						case 2:
							val3.g += 0.51f;
							break;
						case 4:
							val3.r += 0.51f;
							break;
						case 8:
							val3.a += 0.51f;
							break;
						}
						for (int k = 0; k < 4; k++)
						{
							cols.Add((Color32)(val3));
						}
					}
				}
				else
				{
					zero.x = val.x * (float)num3;
					zero.y = (0f - val.y) * (float)num4;
					zero2.x = zero.x + val.x * (float)bMSymbol.width;
					zero2.y = zero.y - val.y * (float)bMSymbol.height;
					zero3.x = mUVRect.xMin + num7 * (float)bMSymbol.x;
					zero3.y = mUVRect.yMax - num8 * (float)bMSymbol.y;
					zero4.x = zero3.x + num7 * (float)bMSymbol.width;
					zero4.y = zero3.y - num8 * (float)bMSymbol.height;
					num3 += mSpacingX + bMSymbol.width;
					i += bMSymbol.length - 1;
					num5 = 0;
					if (symbolStyle == SymbolStyle.Colored)
					{
						for (int l = 0; l < 4; l++)
						{
							cols.Add(color);
						}
					}
					else
					{
						Color32 item = (Color32)(Color.white);
						item.a = color.a;
						for (int m = 0; m < 4; m++)
						{
							cols.Add(item);
						}
					}
				}
				verts.Add(new Vector3(zero2.x, zero.y));
				verts.Add(new Vector3(zero2.x, zero2.y));
				verts.Add(new Vector3(zero.x, zero2.y));
				verts.Add(new Vector3(zero.x, zero.y));
				uvs.Add(new Vector2(zero4.x, zero3.y));
				uvs.Add(new Vector2(zero4.x, zero4.y));
				uvs.Add(new Vector2(zero3.x, zero4.y));
				uvs.Add(new Vector2(zero3.x, zero3.y));
			}
			if (alignment != Alignment.Left && num < verts.size)
			{
				Align(verts, num, alignment, num3, lineWidth);
				num = verts.size;
			}
		}
	}
}
