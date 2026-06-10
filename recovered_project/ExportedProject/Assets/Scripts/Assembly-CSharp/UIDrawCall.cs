using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
	public enum Clipping
	{
		None = 0,
		HardClip = 1,
		AlphaClip = 2,
		SoftClip = 3
	}

	private Transform mTrans;

	private Material mSharedMat;

	private Mesh mMesh0;

	private Mesh mMesh1;

	private MeshFilter mFilter;

	private MeshRenderer mRen;

	private Clipping mClipping;

	private Vector4 mClipRange;

	private Vector2 mClipSoft;

	private Material mClippedMat;

	private Material mDepthMat;

	private int[] mIndices;

	private bool mDepthPass;

	private bool mReset = true;

	private bool mEven = true;

	public bool depthPass
	{
		get
		{
			return mDepthPass;
		}
		set
		{
			if (mDepthPass != value)
			{
				mDepthPass = value;
				mReset = true;
			}
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if ((Object)(object)mTrans == (Object)null)
			{
				mTrans = ((Component)this).transform;
			}
			return mTrans;
		}
	}

	public Material material
	{
		get
		{
			return mSharedMat;
		}
		set
		{
			mSharedMat = value;
		}
	}

	public int triangles
	{
		get
		{
			Mesh val = ((!mEven) ? mMesh1 : mMesh0);
			return ((Object)(object)val != (Object)null) ? (val.vertexCount >> 1) : 0;
		}
	}

	public Clipping clipping
	{
		get
		{
			return mClipping;
		}
		set
		{
			if (mClipping != value)
			{
				mClipping = value;
				mReset = true;
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mClipRange;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			mClipRange = value;
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mClipSoft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			mClipSoft = value;
		}
	}

	private Mesh GetMesh(ref bool rebuildIndices, int vertexCount)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		mEven = !mEven;
		if (mEven)
		{
			if ((Object)(object)mMesh0 == (Object)null)
			{
				mMesh0 = new Mesh();
				((Object)mMesh0).hideFlags = (HideFlags)4;
				((Object)mMesh0).name = "Mesh0 for " + ((Object)mSharedMat).name;
				rebuildIndices = true;
			}
			else if (rebuildIndices || mMesh0.vertexCount != vertexCount)
			{
				rebuildIndices = true;
				mMesh0.Clear();
			}
			return mMesh0;
		}
		if ((Object)(object)mMesh1 == (Object)null)
		{
			mMesh1 = new Mesh();
			((Object)mMesh1).hideFlags = (HideFlags)4;
			((Object)mMesh1).name = "Mesh1 for " + ((Object)mSharedMat).name;
			rebuildIndices = true;
		}
		else if (rebuildIndices || mMesh1.vertexCount != vertexCount)
		{
			rebuildIndices = true;
			mMesh1.Clear();
		}
		return mMesh1;
	}

	private void UpdateMaterials()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		if (mClipping != Clipping.None)
		{
			Shader val = null;
			if (mClipping != Clipping.None)
			{
				string name = ((Object)mSharedMat.shader).name;
				name = name.Replace(" (HardClip)", string.Empty);
				name = name.Replace(" (AlphaClip)", string.Empty);
				name = name.Replace(" (SoftClip)", string.Empty);
				if (mClipping == Clipping.HardClip)
				{
					val = Shader.Find(name + " (HardClip)");
				}
				else if (mClipping == Clipping.AlphaClip)
				{
					val = Shader.Find(name + " (AlphaClip)");
				}
				else if (mClipping == Clipping.SoftClip)
				{
					val = Shader.Find(name + " (SoftClip)");
				}
				if ((Object)(object)val == (Object)null)
				{
					mClipping = Clipping.None;
				}
			}
			if ((Object)(object)val != (Object)null)
			{
				mClippedMat = new Material(mSharedMat);
				((Object)mClippedMat).hideFlags = (HideFlags)4;
				mClippedMat.shader = val;
			}
		}
		else if ((Object)(object)mClippedMat != (Object)null)
		{
			NGUITools.Destroy((Object)(object)mClippedMat);
			mClippedMat = null;
		}
		if (mDepthPass)
		{
			if ((Object)(object)mDepthMat == (Object)null)
			{
				Shader val2 = Shader.Find("Unlit/Depth Cutout");
				mDepthMat = new Material(val2);
				((Object)mDepthMat).hideFlags = (HideFlags)4;
				mDepthMat.mainTexture = mSharedMat.mainTexture;
			}
		}
		else if ((Object)(object)mDepthMat != (Object)null)
		{
			NGUITools.Destroy((Object)(object)mDepthMat);
			mDepthMat = null;
		}
		Material val3 = ((!((Object)(object)mClippedMat != (Object)null)) ? mSharedMat : mClippedMat);
		if ((Object)(object)mDepthMat != (Object)null)
		{
			if (((Renderer)mRen).sharedMaterials == null || ((Renderer)mRen).sharedMaterials.Length != 2 || !((Object)(object)((Renderer)mRen).sharedMaterials[1] == (Object)(object)val3))
			{
				((Renderer)mRen).sharedMaterials = (Material[])(object)new Material[2] { mDepthMat, val3 };
			}
		}
		else if ((Object)(object)((Renderer)mRen).sharedMaterial != (Object)(object)val3)
		{
			((Renderer)mRen).sharedMaterials = (Material[])(object)new Material[1] { val3 };
		}
	}

	public void Set(BetterList<Vector3> verts, BetterList<Vector3> norms, BetterList<Vector4> tans, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		int size = verts.size;
		if (size > 0 && size == uvs.size && size == cols.size && size % 4 == 0)
		{
			if ((Object)(object)mFilter == (Object)null)
			{
				mFilter = ((Component)this).gameObject.GetComponent<MeshFilter>();
			}
			if ((Object)(object)mFilter == (Object)null)
			{
				mFilter = ((Component)this).gameObject.AddComponent<MeshFilter>();
			}
			if ((Object)(object)mRen == (Object)null)
			{
				mRen = ((Component)this).gameObject.GetComponent<MeshRenderer>();
			}
			if ((Object)(object)mRen == (Object)null)
			{
				mRen = ((Component)this).gameObject.AddComponent<MeshRenderer>();
				UpdateMaterials();
			}
			if (verts.size < 65000)
			{
				int num = (size >> 1) * 3;
				bool rebuildIndices = mIndices == null || mIndices.Length != num;
				if (rebuildIndices)
				{
					mIndices = new int[num];
					int num2 = 0;
					for (int i = 0; i < size; i += 4)
					{
						mIndices[num2++] = i;
						mIndices[num2++] = i + 1;
						mIndices[num2++] = i + 2;
						mIndices[num2++] = i + 2;
						mIndices[num2++] = i + 3;
						mIndices[num2++] = i;
					}
				}
				Mesh mesh = GetMesh(ref rebuildIndices, verts.size);
				mesh.vertices = verts.ToArray();
				if (norms != null)
				{
					mesh.normals = norms.ToArray();
				}
				if (tans != null)
				{
					mesh.tangents = tans.ToArray();
				}
				mesh.uv = uvs.ToArray();
				mesh.colors32 = cols.ToArray();
				if (rebuildIndices)
				{
					mesh.triangles = mIndices;
				}
				mesh.RecalculateBounds();
				mFilter.mesh = mesh;
			}
			else
			{
				if ((Object)(object)mFilter.mesh != (Object)null)
				{
					mFilter.mesh.Clear();
				}
				Debug.LogError((object)string.Concat((object)"Too many vertices on one panel: ", (object)verts.size));
			}
		}
		else
		{
			if ((Object)(object)mFilter.mesh != (Object)null)
			{
				mFilter.mesh.Clear();
			}
			Debug.LogError((object)string.Concat((object)"UIWidgets must fill the buffer with 4 vertices per quad. Found ", (object)size));
		}
	}

	private void OnWillRenderObject()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (mReset)
		{
			mReset = false;
			UpdateMaterials();
		}
		if ((Object)(object)mClippedMat != (Object)null)
		{
			mClippedMat.mainTextureOffset = new Vector2((0f - mClipRange.x) / mClipRange.z, (0f - mClipRange.y) / mClipRange.w);
			mClippedMat.mainTextureScale = new Vector2(1f / mClipRange.z, 1f / mClipRange.w);
			Vector2 val = default(Vector2);
			val = new Vector2(1000f, 1000f);
			if (mClipSoft.x > 0f)
			{
				val.x = mClipRange.z / mClipSoft.x;
			}
			if (mClipSoft.y > 0f)
			{
				val.y = mClipRange.w / mClipSoft.y;
			}
			mClippedMat.SetVector("_ClipSharpness", (Vector4)(val));
		}
	}

	private void OnDestroy()
	{
		NGUITools.DestroyImmediate((Object)(object)mMesh0);
		NGUITools.DestroyImmediate((Object)(object)mMesh1);
		NGUITools.DestroyImmediate((Object)(object)mClippedMat);
		NGUITools.DestroyImmediate((Object)(object)mDepthMat);
	}
}
