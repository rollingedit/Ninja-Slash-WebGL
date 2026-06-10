using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class AttachTexture : MonoBehaviour
{
	public GameObject textureToAttach;

	public float attachingTime;

	private void OnTriggerEnter(Collider collider)
	{
		if (((Component)collider).tag.Equals("Player"))
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnAttachTextureEvent(new AttachTextureData(textureToAttach, attachingTime)));
		}
	}
}
