using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ZombieDyingSound : MonoBehaviour
{
	public AudioClip dyingSound;

	private void Awake()
	{
		GetComponent<AudioSource>().clip = dyingSound;
	}

	private void OnZombieCrashed()
	{
		GetComponent<AudioSource>().Play();
	}
}
