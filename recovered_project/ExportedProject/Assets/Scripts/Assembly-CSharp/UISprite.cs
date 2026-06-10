using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Sprite (Basic)")]
[ExecuteInEditMode]
public class UISprite : UIWidget
{
	[HideInInspector]
	[SerializeField]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	protected UIAtlas.Sprite mSprite;

	protected Rect mOuter;

	protected Rect mOuterUV;

	private bool mSpriteSet;

	private string mLastName = string.Empty;

	public Rect outerUV
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			UpdateUVs(false);
			return mOuterUV;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			if ((Object)(object)mAtlas != (Object)(object)value)
			{
				mAtlas = value;
				mSpriteSet = false;
				mSprite = null;
				material = ((!((Object)(object)mAtlas != (Object)null)) ? null : mAtlas.spriteMaterial);
				if (string.IsNullOrEmpty(mSpriteName) && (Object)(object)mAtlas != (Object)null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					string text = mSpriteName;
					mSpriteName = string.Empty;
					spriteName = text;
					mChanged = true;
					UpdateUVs(true);
				}
			}
		}
	}

	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					mSpriteName = string.Empty;
					mSprite = null;
					mChanged = true;
				}
			}
			else if (mSpriteName != value)
			{
				mSpriteName = value;
				mSprite = null;
				mChanged = true;
				if (mSprite != null)
				{
					UpdateUVs(true);
				}
			}
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (!mSpriteSet)
			{
				mSprite = null;
			}
			if (mSprite == null && (Object)(object)mAtlas != (Object)null)
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					sprite = mAtlas.GetSprite(mSpriteName);
				}
				if (mSprite == null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (mSprite != null)
				{
					material = mAtlas.spriteMaterial;
				}
			}
			return mSprite;
		}
		set
		{
			mSprite = value;
			mSpriteSet = true;
			material = ((mSprite == null || !((Object)(object)mAtlas != (Object)null)) ? null : mAtlas.spriteMaterial);
		}
	}

	public override Vector2 pivotOffset
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.zero;
			if (sprite != null)
			{
				Pivot pivot = base.pivot;
				switch (pivot)
				{
				case Pivot.Top:
				case Pivot.Center:
				case Pivot.Bottom:
					zero.x = (-1f - mSprite.paddingRight + mSprite.paddingLeft) * 0.5f;
					break;
				case Pivot.TopRight:
				case Pivot.Right:
				case Pivot.BottomRight:
					zero.x = -1f - mSprite.paddingRight;
					break;
				default:
					zero.x = mSprite.paddingLeft;
					break;
				}
				switch (pivot)
				{
				case Pivot.Left:
				case Pivot.Center:
				case Pivot.Right:
					zero.y = (1f + mSprite.paddingBottom - mSprite.paddingTop) * 0.5f;
					break;
				case Pivot.BottomLeft:
				case Pivot.Bottom:
				case Pivot.BottomRight:
					zero.y = 1f + mSprite.paddingBottom;
					break;
				default:
					zero.y = 0f - mSprite.paddingTop;
					break;
				}
			}
			return zero;
		}
	}

	public override Material material
	{
		get
		{
			Material val = base.material;
			if ((Object)(object)val == (Object)null)
			{
				val = ((!((Object)(object)mAtlas != (Object)null)) ? null : mAtlas.spriteMaterial);
				mSprite = null;
				material = val;
				if ((Object)(object)val != (Object)null)
				{
					UpdateUVs(true);
				}
			}
			return val;
		}
	}

	public virtual Vector4 border
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Vector4.zero;
		}
	}

	public virtual void UpdateUVs(bool force)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (sprite == null || (!force && !(mOuter != mSprite.outer)))
		{
			return;
		}
		Texture val = mainTexture;
		if ((Object)(object)val != (Object)null)
		{
			mOuter = mSprite.outer;
			mOuterUV = mOuter;
			if (mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
			{
				mOuterUV = NGUIMath.ConvertToTexCoords(mOuterUV, val.width, val.height);
			}
			mChanged = true;
		}
	}

	public override void MakePixelPerfect()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		if (sprite != null)
		{
			Texture val = mainTexture;
			Vector3 localScale = base.cachedTransform.localScale;
			if ((Object)(object)val != (Object)null)
			{
				Rect val2 = NGUIMath.ConvertToPixels(outerUV, val.width, val.height, true);
				float pixelSize = atlas.pixelSize;
				localScale.x = (float)Mathf.RoundToInt(val2.width * pixelSize) * Mathf.Sign(localScale.x);
				localScale.y = (float)Mathf.RoundToInt(val2.height * pixelSize) * Mathf.Sign(localScale.y);
				localScale.z = 1f;
				base.cachedTransform.localScale = localScale;
			}
			int num = Mathf.RoundToInt(Mathf.Abs(localScale.x) * (1f + mSprite.paddingLeft + mSprite.paddingRight));
			int num2 = Mathf.RoundToInt(Mathf.Abs(localScale.y) * (1f + mSprite.paddingTop + mSprite.paddingBottom));
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (num % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
			{
				localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
			}
			else
			{
				localPosition.x = Mathf.Round(localPosition.x);
			}
			if (num2 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
			{
				localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
			}
			else
			{
				localPosition.y = Mathf.Round(localPosition.y);
			}
			base.cachedTransform.localPosition = localPosition;
		}
	}

	protected override void OnStart()
	{
		if ((Object)(object)mAtlas != (Object)null)
		{
			UpdateUVs(true);
		}
	}

	public override bool OnUpdate()
	{
		if (mLastName != mSpriteName)
		{
			mSprite = null;
			mChanged = true;
			mLastName = mSpriteName;
			UpdateUVs(false);
			return true;
		}
		UpdateUVs(false);
		return false;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		Vector2 item = default(Vector2);
		item = new Vector2(mOuterUV.xMin, mOuterUV.yMin);
		Vector2 item2 = default(Vector2);
		item2 = new Vector2(mOuterUV.xMax, mOuterUV.yMax);
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(item2);
		uvs.Add(new Vector2(item2.x, item.y));
		uvs.Add(item);
		uvs.Add(new Vector2(item.x, item2.y));
		Color32 item3 = (Color32)(base.color);
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
		cols.Add(item3);
	}
}
