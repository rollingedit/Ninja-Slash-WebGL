using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Sprite (Sliced)")]
public class UISlicedSprite : UISprite
{
	[HideInInspector]
	[SerializeField]
	private bool mFillCenter = true;

	protected Rect mInner;

	protected Rect mInnerUV;

	protected Vector3 mScale = Vector3.one;

	public Rect innerUV
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			UpdateUVs(false);
			return mInnerUV;
		}
	}

	public bool fillCenter
	{
		get
		{
			return mFillCenter;
		}
		set
		{
			if (mFillCenter != value)
			{
				mFillCenter = value;
				MarkAsChanged();
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			UIAtlas.Sprite sprite = base.sprite;
			if (sprite == null)
			{
				return (Vector4)(Vector2.zero);
			}
			Rect rect = sprite.outer;
			Rect rect2 = sprite.inner;
			Texture val = mainTexture;
			if (base.atlas.coordinates == UIAtlas.Coordinates.TexCoords && (Object)(object)val != (Object)null)
			{
				rect = NGUIMath.ConvertToPixels(rect, val.width, val.height, true);
				rect2 = NGUIMath.ConvertToPixels(rect2, val.width, val.height, true);
			}
			return new Vector4(rect2.xMin - rect.xMin, rect2.yMin - rect.yMin, rect.xMax - rect2.xMax, rect.yMax - rect2.yMax) * base.atlas.pixelSize;
		}
	}

	public override Vector2 pivotOffset
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.zero;
			Pivot pivot = base.pivot;
			switch (pivot)
			{
			case Pivot.Top:
			case Pivot.Center:
			case Pivot.Bottom:
				zero.x = -0.5f;
				break;
			case Pivot.TopRight:
			case Pivot.Right:
			case Pivot.BottomRight:
				zero.x = -1f;
				break;
			}
			switch (pivot)
			{
			case Pivot.Left:
			case Pivot.Center:
			case Pivot.Right:
				zero.y = 0.5f;
				break;
			case Pivot.BottomLeft:
			case Pivot.Bottom:
			case Pivot.BottomRight:
				zero.y = 1f;
				break;
			}
			return zero;
		}
	}

	public override void UpdateUVs(bool force)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		if (base.cachedTransform.localScale != mScale)
		{
			mScale = base.cachedTransform.localScale;
			mChanged = true;
		}
		if (base.sprite == null || (!force && !(mInner != mSprite.inner) && !(mOuter != mSprite.outer)))
		{
			return;
		}
		Texture val = mainTexture;
		if ((Object)(object)val != (Object)null)
		{
			mInner = mSprite.inner;
			mOuter = mSprite.outer;
			mInnerUV = mInner;
			mOuterUV = mOuter;
			if (base.atlas.coordinates == UIAtlas.Coordinates.Pixels)
			{
				mOuterUV = NGUIMath.ConvertToTexCoords(mOuterUV, val.width, val.height);
				mInnerUV = NGUIMath.ConvertToTexCoords(mInnerUV, val.width, val.height);
			}
		}
	}

	public override void MakePixelPerfect()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.x = Mathf.RoundToInt(localPosition.x);
		localPosition.y = Mathf.RoundToInt(localPosition.y);
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		localScale.x = Mathf.RoundToInt(localScale.x * 0.5f) << 1;
		localScale.y = Mathf.RoundToInt(localScale.y * 0.5f) << 1;
		localScale.z = 1f;
		base.cachedTransform.localScale = localScale;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		if (mOuterUV == mInnerUV)
		{
			base.OnFill(verts, uvs, cols);
			return;
		}
		Vector2[] array = (Vector2[])(object)new Vector2[4];
		Vector2[] array2 = (Vector2[])(object)new Vector2[4];
		Texture val = mainTexture;
		array[0] = Vector2.zero;
		array[1] = Vector2.zero;
		array[2] = new Vector2(1f, -1f);
		array[3] = new Vector2(1f, -1f);
		if ((Object)(object)val != (Object)null)
		{
			float pixelSize = base.atlas.pixelSize;
			float num = (mInnerUV.xMin - mOuterUV.xMin) * pixelSize;
			float num2 = (mOuterUV.xMax - mInnerUV.xMax) * pixelSize;
			float num3 = (mInnerUV.yMax - mOuterUV.yMax) * pixelSize;
			float num4 = (mOuterUV.yMin - mInnerUV.yMin) * pixelSize;
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = Mathf.Max(0f, localScale.x);
			localScale.y = Mathf.Max(0f, localScale.y);
			Vector2 val2 = default(Vector2);
			val2 = new Vector2(localScale.x / (float)val.width, localScale.y / (float)val.height);
			Vector2 val3 = default(Vector2);
			val3 = new Vector2(num / val2.x, num3 / val2.y);
			Vector2 val4 = default(Vector2);
			val4 = new Vector2(num2 / val2.x, num4 / val2.y);
			Pivot pivot = base.pivot;
			if (pivot == Pivot.Right || pivot == Pivot.TopRight || pivot == Pivot.BottomRight)
			{
				array[0].x = Mathf.Min(0f, 1f - (val4.x + val3.x));
				array[1].x = array[0].x + val3.x;
				array[2].x = array[0].x + Mathf.Max(val3.x, 1f - val4.x);
				array[3].x = array[0].x + Mathf.Max(val3.x + val4.x, 1f);
			}
			else
			{
				array[1].x = val3.x;
				array[2].x = Mathf.Max(val3.x, 1f - val4.x);
				array[3].x = Mathf.Max(val3.x + val4.x, 1f);
			}
			if (pivot == Pivot.Bottom || pivot == Pivot.BottomLeft || pivot == Pivot.BottomRight)
			{
				array[0].y = Mathf.Max(0f, -1f - (val4.y + val3.y));
				array[1].y = array[0].y + val3.y;
				array[2].y = array[0].y + Mathf.Min(val3.y, -1f - val4.y);
				array[3].y = array[0].y + Mathf.Min(val3.y + val4.y, -1f);
			}
			else
			{
				array[1].y = val3.y;
				array[2].y = Mathf.Min(val3.y, -1f - val4.y);
				array[3].y = Mathf.Min(val3.y + val4.y, -1f);
			}
			array2[0] = new Vector2(mOuterUV.xMin, mOuterUV.yMax);
			array2[1] = new Vector2(mInnerUV.xMin, mInnerUV.yMax);
			array2[2] = new Vector2(mInnerUV.xMax, mInnerUV.yMin);
			array2[3] = new Vector2(mOuterUV.xMax, mOuterUV.yMin);
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				array2[i] = Vector2.zero;
			}
		}
		Color32 item = (Color32)(base.color);
		for (int j = 0; j < 3; j++)
		{
			int num5 = j + 1;
			for (int k = 0; k < 3; k++)
			{
				if (mFillCenter || j != 1 || k != 1)
				{
					int num6 = k + 1;
					verts.Add(new Vector3(array[num5].x, array[k].y, 0f));
					verts.Add(new Vector3(array[num5].x, array[num6].y, 0f));
					verts.Add(new Vector3(array[j].x, array[num6].y, 0f));
					verts.Add(new Vector3(array[j].x, array[k].y, 0f));
					uvs.Add(new Vector2(array2[num5].x, array2[k].y));
					uvs.Add(new Vector2(array2[num5].x, array2[num6].y));
					uvs.Add(new Vector2(array2[j].x, array2[num6].y));
					uvs.Add(new Vector2(array2[j].x, array2[k].y));
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
				}
			}
		}
	}
}
