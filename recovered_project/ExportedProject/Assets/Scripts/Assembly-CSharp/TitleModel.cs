using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TitleModel : MonoBehaviour
{
	private GameObject modelInst;

	private void Start()
	{
		SetModel();
	}

	private void OnChangeModel()
	{
		PoolManager.Despawn(modelInst);
		SetModel();
	}

	private void SetModel()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		modelInst = PoolManager.SpawnAndAttachToParent(MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja().ninjaModel, ((Component)this).gameObject);
		modelInst.transform.localPosition = Vector3.zero;
	}
}
