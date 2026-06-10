using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PoolParticleEffect : MonoBehaviour
{
	private ParticleSystem ps;

	private void Awake()
	{
		ps = ((Component)this).GetComponent<ParticleSystem>();
	}

	private void OnGetObjectFromPool()
	{
		ps.Play();
		((MonoBehaviour)this).StartCoroutine("WaitAndPool");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndPool()
	{
		yield return (object)new WaitForSeconds(ps.duration);
		ps.Stop();
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
