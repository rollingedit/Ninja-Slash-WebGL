using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Sprite (Filled)")]
public class UIFilledSprite : UISprite
{
	public enum FillDirection
	{
		Horizontal = 0,
		Vertical = 1,
		Radial90 = 2,
		Radial180 = 3,
		Radial360 = 4
	}

	[HideInInspector]
	[SerializeField]
	private FillDirection mFillDirection = FillDirection.Radial360;

	[SerializeField]
	[HideInInspector]
	private float mFillAmount = 1f;

	[SerializeField]
	[HideInInspector]
	private bool mInvert;

	public FillDirection fillDirection
	{
		get
		{
			return mFillDirection;
		}
		set
		{
			if (mFillDirection != value)
			{
				mFillDirection = value;
				mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mFillAmount != num)
			{
				mFillAmount = num;
				mChanged = true;
			}
		}
	}

	public bool invert
	{
		get
		{
			return mInvert;
		}
		set
		{
			if (mInvert != value)
			{
				mInvert = value;
				mChanged = true;
			}
		}
	}

	private bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (!invert)
		{
			num = 1f - num;
		}
		num *= (float)Math.PI / 2f;
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Cos(num);
		if (num2 > num3)
		{
			num3 *= 1f / num2;
			num2 = 1f;
			if (!invert)
			{
				xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
				xy[3].y = xy[0].y;
				uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
				uv[3].y = uv[0].y;
			}
		}
		else if (num3 > num2)
		{
			num2 *= 1f / num3;
			num3 = 1f;
			if (invert)
			{
				xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
				xy[1].x = xy[0].x;
				uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
				uv[1].x = uv[0].x;
			}
		}
		else
		{
			num2 = 1f;
			num3 = 1f;
		}
		if (invert)
		{
			xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
			uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
		}
		else
		{
			xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
			uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
		}
		return true;
	}

	private void Rotate(Vector2[] v, int offset)
	{
		Vector2 val = default(Vector2);
		for (int i = 0; i < offset; i++)
		{
			val = new Vector2(v[3].x, v[3].y);
			v[3].x = v[2].y;
			v[3].y = v[2].x;
			v[2].x = v[1].y;
			v[2].y = v[1].x;
			v[1].x = v[0].y;
			v[1].y = v[0].x;
			v[0].x = val.y;
			v[0].y = val.x;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = -1f;
		float num5 = mOuterUV.xMin;
		float num6 = mOuterUV.yMin;
		float num7 = mOuterUV.xMax;
		float num8 = mOuterUV.yMax;
		if (mFillDirection == FillDirection.Horizontal || mFillDirection == FillDirection.Vertical)
		{
			float num9 = (num7 - num5) * mFillAmount;
			float num10 = (num8 - num6) * mFillAmount;
			if (fillDirection == FillDirection.Horizontal)
			{
				if (mInvert)
				{
					num = 1f - mFillAmount;
					num5 = num7 - num9;
				}
				else
				{
					num3 *= mFillAmount;
					num7 = num5 + num9;
				}
			}
			else if (fillDirection == FillDirection.Vertical)
			{
				if (mInvert)
				{
					num4 *= mFillAmount;
					num6 = num8 - num10;
				}
				else
				{
					num2 = 0f - (1f - mFillAmount);
					num8 = num6 + num10;
				}
			}
		}
		Vector2[] array = (Vector2[])(object)new Vector2[4];
		Vector2[] array2 = (Vector2[])(object)new Vector2[4];
		array[0] = new Vector2(num3, num2);
		array[1] = new Vector2(num3, num4);
		array[2] = new Vector2(num, num4);
		array[3] = new Vector2(num, num2);
		array2[0] = new Vector2(num7, num8);
		array2[1] = new Vector2(num7, num6);
		array2[2] = new Vector2(num5, num6);
		array2[3] = new Vector2(num5, num8);
		Color32 item = (Color32)(base.color);
		if (fillDirection == FillDirection.Radial90)
		{
			if (!AdjustRadial(array, array2, mFillAmount, mInvert))
			{
				return;
			}
		}
		else
		{
			if (fillDirection == FillDirection.Radial180)
			{
				Vector2[] array3 = (Vector2[])(object)new Vector2[4];
				Vector2[] array4 = (Vector2[])(object)new Vector2[4];
				for (int i = 0; i < 2; i++)
				{
					array3[0] = new Vector2(0f, 0f);
					array3[1] = new Vector2(0f, 1f);
					array3[2] = new Vector2(1f, 1f);
					array3[3] = new Vector2(1f, 0f);
					array4[0] = new Vector2(0f, 0f);
					array4[1] = new Vector2(0f, 1f);
					array4[2] = new Vector2(1f, 1f);
					array4[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (i > 0)
						{
							Rotate(array3, i);
							Rotate(array4, i);
						}
					}
					else if (i < 1)
					{
						Rotate(array3, 1 - i);
						Rotate(array4, 1 - i);
					}
					float num11;
					float num12;
					if (i == 1)
					{
						num11 = ((!mInvert) ? 1f : 0.5f);
						num12 = ((!mInvert) ? 0.5f : 1f);
					}
					else
					{
						num11 = ((!mInvert) ? 0.5f : 1f);
						num12 = ((!mInvert) ? 1f : 0.5f);
					}
					array3[1].y = Mathf.Lerp(num11, num12, array3[1].y);
					array3[2].y = Mathf.Lerp(num11, num12, array3[2].y);
					array4[1].y = Mathf.Lerp(num11, num12, array4[1].y);
					array4[2].y = Mathf.Lerp(num11, num12, array4[2].y);
					float fill = mFillAmount * 2f - (float)i;
					bool flag = i % 2 == 1;
					if (!AdjustRadial(array3, array4, fill, !flag))
					{
						continue;
					}
					if (mInvert)
					{
						flag = !flag;
					}
					if (flag)
					{
						for (int j = 0; j < 4; j++)
						{
							num11 = Mathf.Lerp(array[0].x, array[2].x, array3[j].x);
							num12 = Mathf.Lerp(array[0].y, array[2].y, array3[j].y);
							float num13 = Mathf.Lerp(array2[0].x, array2[2].x, array4[j].x);
							float num14 = Mathf.Lerp(array2[0].y, array2[2].y, array4[j].y);
							verts.Add(new Vector3(num11, num12, 0f));
							uvs.Add(new Vector2(num13, num14));
							cols.Add(item);
						}
						continue;
					}
					for (int num15 = 3; num15 > -1; num15--)
					{
						num11 = Mathf.Lerp(array[0].x, array[2].x, array3[num15].x);
						num12 = Mathf.Lerp(array[0].y, array[2].y, array3[num15].y);
						float num16 = Mathf.Lerp(array2[0].x, array2[2].x, array4[num15].x);
						float num17 = Mathf.Lerp(array2[0].y, array2[2].y, array4[num15].y);
						verts.Add(new Vector3(num11, num12, 0f));
						uvs.Add(new Vector2(num16, num17));
						cols.Add(item);
					}
				}
				return;
			}
			if (fillDirection == FillDirection.Radial360)
			{
				float[] array5 = new float[16]
				{
					0.5f, 1f, 0.5f, 1f,
					0f, 0.5f, 0.5f, 1f,
					0f, 0.5f, 0f, 0.5f,
					0.5f, 1f, 0f, 0.5f
				};
				float[] array6 = array5;
				Vector2[] array7 = (Vector2[])(object)new Vector2[4];
				Vector2[] array8 = (Vector2[])(object)new Vector2[4];
				for (int k = 0; k < 4; k++)
				{
					array7[0] = new Vector2(0f, 0f);
					array7[1] = new Vector2(0f, 1f);
					array7[2] = new Vector2(1f, 1f);
					array7[3] = new Vector2(1f, 0f);
					array8[0] = new Vector2(0f, 0f);
					array8[1] = new Vector2(0f, 1f);
					array8[2] = new Vector2(1f, 1f);
					array8[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (k > 0)
						{
							Rotate(array7, k);
							Rotate(array8, k);
						}
					}
					else if (k < 3)
					{
						Rotate(array7, 3 - k);
						Rotate(array8, 3 - k);
					}
					for (int l = 0; l < 4; l++)
					{
						int num18 = ((!mInvert) ? (k * 4) : ((3 - k) * 4));
						float num19 = array6[num18];
						float num20 = array6[num18 + 1];
						float num21 = array6[num18 + 2];
						float num22 = array6[num18 + 3];
						array7[l].x = Mathf.Lerp(num19, num20, array7[l].x);
						array7[l].y = Mathf.Lerp(num21, num22, array7[l].y);
						array8[l].x = Mathf.Lerp(num19, num20, array8[l].x);
						array8[l].y = Mathf.Lerp(num21, num22, array8[l].y);
					}
					float fill2 = mFillAmount * 4f - (float)k;
					bool flag2 = k % 2 == 1;
					if (!AdjustRadial(array7, array8, fill2, !flag2))
					{
						continue;
					}
					if (mInvert)
					{
						flag2 = !flag2;
					}
					if (flag2)
					{
						for (int m = 0; m < 4; m++)
						{
							float num23 = Mathf.Lerp(array[0].x, array[2].x, array7[m].x);
							float num24 = Mathf.Lerp(array[0].y, array[2].y, array7[m].y);
							float num25 = Mathf.Lerp(array2[0].x, array2[2].x, array8[m].x);
							float num26 = Mathf.Lerp(array2[0].y, array2[2].y, array8[m].y);
							verts.Add(new Vector3(num23, num24, 0f));
							uvs.Add(new Vector2(num25, num26));
							cols.Add(item);
						}
						continue;
					}
					for (int num27 = 3; num27 > -1; num27--)
					{
						float num28 = Mathf.Lerp(array[0].x, array[2].x, array7[num27].x);
						float num29 = Mathf.Lerp(array[0].y, array[2].y, array7[num27].y);
						float num30 = Mathf.Lerp(array2[0].x, array2[2].x, array8[num27].x);
						float num31 = Mathf.Lerp(array2[0].y, array2[2].y, array8[num27].y);
						verts.Add(new Vector3(num28, num29, 0f));
						uvs.Add(new Vector2(num30, num31));
						cols.Add(item);
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add((Vector2)(array[n]));
			uvs.Add(array2[n]);
			cols.Add(item);
		}
	}
}
