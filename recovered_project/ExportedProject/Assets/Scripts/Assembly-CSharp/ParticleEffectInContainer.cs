using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ParticleEffectInContainer : MonoBehaviour
{
	private ParticleSystem ps;

	private void Awake()
	{
		ps = ((Component)this).GetComponent<ParticleSystem>();
	}

	private void OnGetObjectFromPool()
	{
		((MonoBehaviour)this).StartCoroutine("WaitAndPool");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndPool()
	{
		yield return (object)new WaitForSeconds(ps.duration);
		((Component)this).SendMessageUpwards("OnFinishEffect");
	}

	private void OnPlayerPooled()
	{
		((MonoBehaviour)this).StopCoroutine("WaitAndPool");
		ps.Stop();
		((Component)this).SendMessageUpwards("OnFinishEffect");
	}
}
