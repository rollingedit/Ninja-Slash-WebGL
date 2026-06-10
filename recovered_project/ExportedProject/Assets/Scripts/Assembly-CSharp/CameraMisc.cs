using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CameraMisc : MonoBehaviour
{
	public GameObject sakuraObject;

	private void SetSakura(bool enableIn)
	{
		sakuraObject.SetActive(enableIn);
	}
}
