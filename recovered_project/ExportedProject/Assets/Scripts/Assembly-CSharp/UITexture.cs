using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Texture")]
[ExecuteInEditMode]
public class UITexture : UIWidget
{
	[HideInInspector]
	[SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	[HideInInspector]
	private Shader mShader;

	[SerializeField]
	[HideInInspector]
	private Texture mTexture;

	private Material mDynamicMat;

	private bool mCreatingMat;

	public Rect uvRect
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mRect;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (mRect != value)
			{
				mRect = value;
				MarkAsChanged();
			}
		}
	}

	public Shader shader
	{
		get
		{
			if ((Object)(object)mShader == (Object)null)
			{
				Material val = material;
				if ((Object)(object)val != (Object)null)
				{
					mShader = val.shader;
				}
				if ((Object)(object)mShader == (Object)null)
				{
					mShader = Shader.Find("Unlit/Texture");
				}
			}
			return mShader;
		}
		set
		{
			if ((Object)(object)mShader != (Object)(object)value)
			{
				mShader = value;
				Material val = material;
				if ((Object)(object)val != (Object)null)
				{
					val.shader = value;
				}
			}
		}
	}

	public bool hasDynamicMaterial
	{
		get
		{
			return (Object)(object)mDynamicMat != (Object)null;
		}
	}

	public override bool keepMaterial
	{
		get
		{
			return true;
		}
	}

	public override Material material
	{
		get
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			if (!mCreatingMat && (Object)(object)base.material == (Object)null)
			{
				mCreatingMat = true;
				if ((Object)(object)mainTexture != (Object)null)
				{
					if ((Object)(object)mShader == (Object)null)
					{
						mShader = Shader.Find("Unlit/Texture");
					}
					mDynamicMat = new Material(mShader);
					((Object)mDynamicMat).hideFlags = (HideFlags)4;
					mDynamicMat.mainTexture = mainTexture;
					base.material = mDynamicMat;
				}
				mCreatingMat = false;
			}
			return base.material;
		}
		set
		{
			if ((Object)(object)mDynamicMat != (Object)(object)value && (Object)(object)mDynamicMat != (Object)null)
			{
				NGUITools.Destroy((Object)(object)mDynamicMat);
				mDynamicMat = null;
			}
			base.material = value;
		}
	}

	public override Texture mainTexture
	{
		get
		{
			return (!((Object)(object)mTexture != (Object)null)) ? base.mainTexture : mTexture;
		}
		set
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			mTexture = value;
			if ((Object)(object)material == (Object)null)
			{
				mDynamicMat = new Material(shader);
				((Object)mDynamicMat).hideFlags = (HideFlags)4;
				mDynamicMat.mainTexture = mainTexture;
				material = mDynamicMat;
			}
			base.mainTexture = value;
		}
	}

	private void OnDestroy()
	{
		NGUITools.Destroy((Object)(object)mDynamicMat);
	}

	public override void MakePixelPerfect()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Texture val = mainTexture;
		if ((Object)(object)val != (Object)null)
		{
			Vector3 localScale = base.cachedTransform.localScale;
			float num = val.width;
			Rect val2 = uvRect;
			localScale.x = num * val2.width;
			float num2 = val.height;
			Rect val3 = uvRect;
			localScale.y = num2 * val3.height;
			localScale.z = 1f;
			base.cachedTransform.localScale = localScale;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(new Vector2(mRect.xMax, mRect.yMax));
		uvs.Add(new Vector2(mRect.xMax, mRect.yMin));
		uvs.Add(new Vector2(mRect.xMin, mRect.yMin));
		uvs.Add(new Vector2(mRect.xMin, mRect.yMax));
		cols.Add((Color32)(base.color));
		cols.Add((Color32)(base.color));
		cols.Add((Color32)(base.color));
		cols.Add((Color32)(base.color));
	}
}
