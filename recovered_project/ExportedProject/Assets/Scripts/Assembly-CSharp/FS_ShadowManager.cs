using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FS_ShadowManager : MonoBehaviour
{
	private static FS_ShadowManager _manager;

	private Hashtable shadowMeshes = new Hashtable();

	private Hashtable shadowMeshesStatic = new Hashtable();

	private void Start()
	{
		FS_ShadowManager[] array = (FS_ShadowManager[])(object)Object.FindObjectsOfType(typeof(FS_ShadowManager));
		if (array.Length > 1)
		{
			Debug.LogWarning((object)string.Concat((object)"There should only be one FS_ShadowManger in the scene. Found ", (object)array.Length));
		}
	}

	private void OnApplicationQuit()
	{
		shadowMeshes.Clear();
		shadowMeshesStatic.Clear();
	}

	public static FS_ShadowManager Manager()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		if ((Object)(object)_manager == (Object)null)
		{
			FS_ShadowManager fS_ShadowManager = (FS_ShadowManager)(object)Object.FindObjectOfType(typeof(FS_ShadowManager));
			if ((Object)(object)fS_ShadowManager == (Object)null)
			{
				GameObject val = new GameObject("FS_ShadowManager");
				_manager = val.AddComponent<FS_ShadowManager>();
			}
			else
			{
				_manager = fS_ShadowManager;
			}
		}
		return _manager;
	}

	public void registerGeometry(FS_ShadowSimple s, FS_MeshKey meshKey)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		FS_ShadowManagerMesh fS_ShadowManagerMesh;
		if (meshKey.isStatic)
		{
			if (!shadowMeshesStatic.ContainsKey((object)meshKey))
			{
				GameObject val = new GameObject("ShadowMeshStatic_" + ((Object)meshKey.mat).name);
				val.transform.parent = ((Component)this).transform;
				fS_ShadowManagerMesh = val.AddComponent<FS_ShadowManagerMesh>();
				fS_ShadowManagerMesh.shadowMaterial = s.shadowMaterial;
				fS_ShadowManagerMesh.isStatic = true;
				shadowMeshesStatic.Add((object)meshKey, (object)fS_ShadowManagerMesh);
			}
			else
			{
				fS_ShadowManagerMesh = (FS_ShadowManagerMesh)shadowMeshesStatic[(object)meshKey];
			}
		}
		else if (!shadowMeshes.ContainsKey((object)meshKey))
		{
			GameObject val2 = new GameObject("ShadowMesh_" + ((Object)meshKey.mat).name);
			val2.transform.parent = ((Component)this).transform;
			fS_ShadowManagerMesh = val2.AddComponent<FS_ShadowManagerMesh>();
			fS_ShadowManagerMesh.shadowMaterial = s.shadowMaterial;
			fS_ShadowManagerMesh.isStatic = false;
			shadowMeshes.Add((object)meshKey, (object)fS_ShadowManagerMesh);
		}
		else
		{
			fS_ShadowManagerMesh = (FS_ShadowManagerMesh)shadowMeshes[(object)meshKey];
		}
		fS_ShadowManagerMesh.registerGeometry(s);
	}
}
