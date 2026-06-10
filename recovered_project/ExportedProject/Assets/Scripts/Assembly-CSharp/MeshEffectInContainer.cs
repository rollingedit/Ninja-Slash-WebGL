using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MeshEffectInContainer : MonoBehaviour
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
		((Component)this).SendMessageUpwards("OnFinishEffect");
	}

	private void OnPlayerPooled()
	{
		((MonoBehaviour)this).StopCoroutine("WaitAndPool");
		animState.normalizedTime = 0f;
		((Component)this).SendMessageUpwards("OnFinishEffect");
	}
}
