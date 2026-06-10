using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{
	private AudioClip clip;

	private void OnTriggerEnter(Collider colliderIn)
	{
		GetComponent<AudioSource>().Play();
	}
}
