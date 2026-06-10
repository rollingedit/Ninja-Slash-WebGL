using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FS_MeshKey
{
	public bool isStatic;

	public Material mat;

	public FS_MeshKey(Material m, bool s)
	{
		isStatic = s;
		mat = m;
	}

	public virtual bool Equals(object obj)
	{
		if (!(obj is FS_MeshKey))
		{
			return false;
		}
		FS_MeshKey fS_MeshKey = (FS_MeshKey)obj;
		if (fS_MeshKey.isStatic == isStatic && (Object)(object)fS_MeshKey.mat == (Object)(object)mat)
		{
			return true;
		}
		return false;
	}

	public virtual int GetHashCode()
	{
		return isStatic.GetHashCode() ^ ((Object)mat).GetHashCode();
	}
}
