using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class AmbientSound : MonoBehaviour
{
	public float waitTime;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine("PlaySoundPeriodically");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator PlaySoundPeriodically()
	{
		while (true)
		{
			yield return (object)new WaitForSeconds(waitTime);
			GetComponent<AudioSource>().Play();
		}
	}
}
