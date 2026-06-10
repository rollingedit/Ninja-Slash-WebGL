using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ChangePrefabOnTriggerEnter : MonoBehaviour
{
	public GameObject prefab;

	private GameObject changedObject;

	private bool isChanged;

	private void OnTriggerEnter(Collider collider)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)collider).tag.Equals("Player"))
		{
			isChanged = true;
			changedObject = PoolManager.Spawn(prefab, ((Component)this).transform.position);
			changedObject.transform.parent = ((Component)this).transform.parent;
			((Component)this).gameObject.SetActive(false);
		}
	}

	private void OnPutObjectIntoPool()
	{
		if (isChanged)
		{
			PoolManager.Despawn(changedObject);
			isChanged = false;
		}
	}
}
