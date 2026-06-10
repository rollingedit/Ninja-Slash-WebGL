using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterModel : MonoBehaviour
{
	private GameObject modelInst;

	private void OnDead()
	{
		PoolManager.SpawnAndAttachToParent(MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja().ninjaRagdoll, ((Component)this).gameObject);
		PoolManager.Despawn(modelInst);
		modelInst = null;
	}

	private void OnGetObjectFromPool()
	{
		if ((Object)(object)modelInst == (Object)null)
		{
			modelInst = PoolManager.SpawnAndAttachToParent(MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja().ninjaModel, ((Component)this).gameObject);
			modelInst.SendMessage("OnCreatePlayerModel", (object)((Component)this).gameObject);
		}
	}

	private void OnChangeModel()
	{
		PoolManager.Despawn(modelInst);
		modelInst = PoolManager.SpawnAndAttachToParent(MonoSingleton<NinjaInfo>.instance.GetCurrentSelectedNinja().ninjaModel, ((Component)this).gameObject);
		modelInst.SendMessage("OnCreatePlayerModel", (object)((Component)this).gameObject);
	}
}
