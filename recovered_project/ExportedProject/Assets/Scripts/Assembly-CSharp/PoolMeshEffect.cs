using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PoolMeshEffect : MonoBehaviour
{
	private AnimationState animState;

	private void Awake()
	{
		animState = GetComponent<Animation>()[((Object)GetComponent<Animation>().clip).name];
	}

	private void OnGetObjectFromPool()
	{
		((MonoBehaviour)this).StartCoroutine("WaitAndPool");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndPool()
	{
		yield return (object)new WaitForSeconds(animState.length);
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
