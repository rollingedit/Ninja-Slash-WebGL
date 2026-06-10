using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class LuckyBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider colliderIn)
	{
		if (((Component)colliderIn).tag == "Player")
		{
			MonoSingleton<LuckyBoxManager>.instance.OnGetLuckyBox();
			((Component)colliderIn).gameObject.BroadcastMessage("OnEnterLuckyBox", (object)((Component)this).gameObject);
		}
	}
}
