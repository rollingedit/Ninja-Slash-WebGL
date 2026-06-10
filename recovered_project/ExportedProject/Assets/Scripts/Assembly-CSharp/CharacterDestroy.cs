using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterDestroy : MonoBehaviour
{
	public float durationUntilGameOver;

	public float delay;

	private void OnPutObjectIntoPool()
	{
		((Component)this).BroadcastMessage("OnPlayerPooled", (SendMessageOptions)1);
	}

	private void OnDead()
	{
		if (!MonoSingleton<UserData>.instance.IsTutorialDone)
		{
			((MonoBehaviour)this).StartCoroutine("SendTutorialGameOverEvent");
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine("SendGameOverEvent");
		}
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator SendGameOverEvent()
	{
		yield return (object)new WaitForSeconds(durationUntilGameOver);
		((Component)this).BroadcastMessage("OnGameOver");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator SendTutorialGameOverEvent()
	{
		yield return (object)new WaitForSeconds(durationUntilGameOver + delay);
		((Component)this).SendMessageUpwards("OnTutorialGameOver");
	}

	private void OnGameOver()
	{
		((MonoBehaviour)this).StartCoroutine("DespawnAfterDelay");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator DespawnAfterDelay()
	{
		yield return (object)new WaitForSeconds(delay);
		((Component)this).transform.position = Vector3.zero;
		PoolManager.Despawn(((Component)this).gameObject);
	}
}
