using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Sprite (Tiled)")]
public class UITiledSprite : UISlicedSprite
{
	public override Vector4 border
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Vector4.zero;
		}
	}

	public override void MakePixelPerfect()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.x = Mathf.RoundToInt(localPosition.x);
		localPosition.y = Mathf.RoundToInt(localPosition.y);
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		localScale.x = Mathf.RoundToInt(localScale.x);
		localScale.y = Mathf.RoundToInt(localScale.y);
		localScale.z = 1f;
		base.cachedTransform.localScale = localScale;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		Texture val = material.mainTexture;
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		Rect rect = mInner;
		if (base.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
		{
			rect = NGUIMath.ConvertToPixels(rect, val.width, val.height, true);
		}
		Vector2 val2 = (Vector2)(base.cachedTransform.localScale);
		float pixelSize = base.atlas.pixelSize;
		float num = Mathf.Abs(rect.width / val2.x) * pixelSize;
		float num2 = Mathf.Abs(rect.height / val2.y) * pixelSize;
		if (num < 0.01f || num2 < 0.01f)
		{
			Debug.LogWarning((object)("The tiled sprite (" + NGUITools.GetHierarchy(((Component)this).gameObject) + ") is too small.\nConsider using a bigger one."));
			num = 0.01f;
			num2 = 0.01f;
		}
		Vector2 val3 = default(Vector2);
		val3 = new Vector2(rect.xMin / (float)val.width, rect.yMin / (float)val.height);
		Vector2 val4 = default(Vector2);
		val4 = new Vector2(rect.xMax / (float)val.width, rect.yMax / (float)val.height);
		Vector2 val5 = val4;
		for (float num3 = 0f; num3 < 1f; num3 += num2)
		{
			float num4 = 0f;
			val5.x = val4.x;
			float num5 = num3 + num2;
			if (num5 > 1f)
			{
				val5.y = val3.y + (val4.y - val3.y) * (1f - num3) / (num5 - num3);
				num5 = 1f;
			}
			for (; num4 < 1f; num4 += num)
			{
				float num6 = num4 + num;
				if (num6 > 1f)
				{
					val5.x = val3.x + (val4.x - val3.x) * (1f - num4) / (num6 - num4);
					num6 = 1f;
				}
				verts.Add(new Vector3(num6, 0f - num3, 0f));
				verts.Add(new Vector3(num6, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num3, 0f));
				uvs.Add(new Vector2(val5.x, 1f - val3.y));
				uvs.Add(new Vector2(val5.x, 1f - val5.y));
				uvs.Add(new Vector2(val3.x, 1f - val5.y));
				uvs.Add(new Vector2(val3.x, 1f - val3.y));
				cols.Add((Color32)(base.color));
				cols.Add((Color32)(base.color));
				cols.Add((Color32)(base.color));
				cols.Add((Color32)(base.color));
			}
		}
	}
}
