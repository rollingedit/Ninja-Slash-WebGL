using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FS_ShadowManagerMesh : MonoBehaviour
{
	public Material shadowMaterial;

	public bool isStatic;

	private int numShadows;

	private List<FS_ShadowSimple> shadows = new List<FS_ShadowSimple>();

	private Mesh _mesh;

	private Mesh _mesh1;

	private Mesh _mesh2;

	private bool pingPong;

	private MeshFilter _filter;

	private Renderer _ren;

	private Vector3[] _verts;

	private Vector3[] _norms;

	private Vector2[] _uvs;

	private Color[] _colors;

	private int[] _indices;

	public int getNumShadows()
	{
		return numShadows;
	}

	public void Start()
	{
		if (isStatic)
		{
			_CreateGeometry();
		}
	}

	public void registerGeometry(FS_ShadowSimple s)
	{
		if ((Object)(object)s.shadowMaterial != (Object)(object)shadowMaterial)
		{
			Debug.LogError((object)"Shadow did not have the same material");
		}
		shadows.Add(s);
	}

	public void recreateStaticGeometry()
	{
		_CreateGeometry();
	}

	private void LateUpdate()
	{
		if (!isStatic)
		{
			_CreateGeometry();
		}
	}

	private Mesh _GetMesh()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		pingPong = !pingPong;
		if (pingPong)
		{
			if ((Object)(object)_mesh1 == (Object)null)
			{
				_mesh1 = new Mesh();
				((Object)_mesh1).hideFlags = (HideFlags)4;
			}
			else
			{
				_mesh1.Clear();
			}
			return _mesh1;
		}
		if ((Object)(object)_mesh2 == (Object)null)
		{
			_mesh2 = new Mesh();
			((Object)_mesh2).hideFlags = (HideFlags)4;
		}
		else
		{
			_mesh2.Clear();
		}
		return _mesh2;
	}

	private void _CreateGeometry()
	{
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		numShadows = shadows.Count;
		int num = shadows.Count * 4;
		_mesh = _GetMesh();
		if ((Object)(object)_filter == (Object)null)
		{
			_filter = ((Component)this).GetComponent<MeshFilter>();
		}
		if ((Object)(object)_filter == (Object)null)
		{
			_filter = ((Component)this).gameObject.AddComponent<MeshFilter>();
		}
		if ((Object)(object)_ren == (Object)null)
		{
			_ren = (Renderer)(object)((Component)this).gameObject.GetComponent<MeshRenderer>();
		}
		if ((Object)(object)_ren == (Object)null)
		{
			_ren = (Renderer)(object)((Component)this).gameObject.AddComponent<MeshRenderer>();
			_ren.material = shadowMaterial;
		}
		if (num < 65000)
		{
			int num2 = (num >> 1) * 3;
			if (_verts == null || _verts.Length != num)
			{
				_indices = new int[num2];
				_verts = (Vector3[])(object)new Vector3[num];
				_norms = (Vector3[])(object)new Vector3[num];
				_uvs = (Vector2[])(object)new Vector2[num];
				_colors = (Color[])(object)new Color[num];
			}
			int num4;
			int num3 = (num4 = 0);
			for (int i = 0; i < shadows.Count; i++)
			{
				FS_ShadowSimple fS_ShadowSimple = shadows[i];
				_verts[num3] = fS_ShadowSimple.corners[0];
				_verts[num3 + 1] = fS_ShadowSimple.corners[1];
				_verts[num3 + 2] = fS_ShadowSimple.corners[2];
				_verts[num3 + 3] = fS_ShadowSimple.corners[3];
				_indices[num4] = num3;
				_indices[num4 + 1] = num3 + 1;
				_indices[num4 + 2] = num3 + 2;
				_indices[num4 + 3] = num3 + 2;
				_indices[num4 + 4] = num3 + 3;
				_indices[num4 + 5] = num3;
				_norms[num3] = fS_ShadowSimple.normal;
				_norms[num3 + 1] = fS_ShadowSimple.normal;
				_norms[num3 + 2] = fS_ShadowSimple.normal;
				_norms[num3 + 3] = fS_ShadowSimple.normal;
				_uvs[num3].x = fS_ShadowSimple.uvs.x;
				_uvs[num3].y = fS_ShadowSimple.uvs.y;
				_uvs[num3 + 1].x = fS_ShadowSimple.uvs.x + fS_ShadowSimple.uvs.width;
				_uvs[num3 + 1].y = fS_ShadowSimple.uvs.y;
				_uvs[num3 + 2].x = fS_ShadowSimple.uvs.x + fS_ShadowSimple.uvs.width;
				_uvs[num3 + 2].y = fS_ShadowSimple.uvs.y + fS_ShadowSimple.uvs.height;
				_uvs[num3 + 3].x = fS_ShadowSimple.uvs.x;
				_uvs[num3 + 3].y = fS_ShadowSimple.uvs.y + fS_ShadowSimple.uvs.height;
				_colors[num3] = fS_ShadowSimple.color;
				_colors[num3 + 1] = fS_ShadowSimple.color;
				_colors[num3 + 2] = fS_ShadowSimple.color;
				_colors[num3 + 3] = fS_ShadowSimple.color;
				num4 += 6;
				num3 += 4;
			}
			_mesh.Clear();
			((Object)_mesh).name = "shadow mesh";
			_mesh.vertices = _verts;
			_mesh.normals = _norms;
			_mesh.uv = _uvs;
			_mesh.triangles = _indices;
			_mesh.colors = _colors;
			_mesh.RecalculateBounds();
			_filter.mesh = _mesh;
			shadows.Clear();
		}
		else
		{
			if ((Object)(object)_filter.mesh != (Object)null)
			{
				_filter.mesh.Clear();
			}
			Debug.LogError((object)string.Concat((object)"Too many shadows. limit is ", (object)16250));
		}
	}
}
