using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BaseEventListener : MonoBehaviour, IEventListener
{
	public string eventName;

	private EventManager eventManager;

	private void Start()
	{
		eventManager = MonoSingleton<EventManager>.instance;
		eventManager.AddListener(this, eventName);
	}

	public bool isEnabled()
	{
		return ((Component)this).gameObject.activeInHierarchy;
	}

	private void OnDestroy()
	{
		if ((Object)(object)eventManager != (Object)null)
		{
			MonoSingleton<EventManager>.instance.DetachListener(this, eventName);
		}
	}

	public virtual bool HandleEvent(IEvent evt)
	{
		string text = evt.GetData() as string;
		Debug.Log((object)("Event received: " + evt.GetName() + " with data: " + text));
		return false;
	}
}
