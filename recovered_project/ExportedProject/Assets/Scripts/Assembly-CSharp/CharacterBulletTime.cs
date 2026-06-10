using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterBulletTime : MonoBehaviour
{
	public float deadTimeScale;

	public float deadDuration;

	private void OnDead()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnChangeTimeScaleEvent(deadTimeScale, deadDuration));
	}
}
