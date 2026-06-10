using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class EventManager : MonoSingleton<EventManager>
{
	public bool LimitQueueProcesing;

	public float QueueProcessTime;

	private Hashtable m_listenerTable = new Hashtable();

	private Queue m_eventQueue = new Queue();

	public bool AddListener(IEventListener listener, string eventName)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		if (listener == null || eventName == null)
		{
			Debug.Log((object)"Event Manager: AddListener failed due to no listener or event name specified.");
			return false;
		}
		if (!m_listenerTable.ContainsKey((object)eventName))
		{
			m_listenerTable.Add((object)eventName, (object)new ArrayList());
		}
		object obj = m_listenerTable[(object)eventName];
		ArrayList val = (ArrayList)((obj is ArrayList) ? obj : null);
		if (val.Contains((object)listener))
		{
			Debug.Log((object)("Event Manager: Listener: " + ((object)listener).GetType().ToString() + " is already in list for event: " + eventName));
			return false;
		}
		val.Add((object)listener);
		return true;
	}

	public bool DetachListener(IEventListener listener, string eventName)
	{
		if (!m_listenerTable.ContainsKey((object)eventName))
		{
			return false;
		}
		object obj = m_listenerTable[(object)eventName];
		ArrayList val = (ArrayList)((obj is ArrayList) ? obj : null);
		if (!val.Contains((object)listener))
		{
			return false;
		}
		val.Remove((object)listener);
		return true;
	}

	public bool TriggerEvent(IEvent evt)
	{
		string name = evt.GetName();
		if (!m_listenerTable.ContainsKey((object)name))
		{
			Debug.Log((object)("Event Manager: Event \"" + name + "\" triggered has no listeners!"));
			return false;
		}
		object obj = m_listenerTable[(object)name];
		ArrayList val = (ArrayList)((obj is ArrayList) ? obj : null);
		foreach (IEventListener item in val)
		{
			if (!item.isEnabled() || !item.HandleEvent(evt))
			{
				continue;
			}
			return true;
		}
		return true;
	}

	public bool QueueEvent(IEvent evt)
	{
		if (!m_listenerTable.ContainsKey((object)evt.GetName()))
		{
			Debug.Log((object)("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetName()));
			return false;
		}
		m_eventQueue.Enqueue((object)evt);
		return true;
	}

	private void Update()
	{
		float num = 0f;
		while (m_eventQueue.Count > 0 && (!LimitQueueProcesing || !(num > QueueProcessTime)))
		{
			IEvent obj = m_eventQueue.Dequeue() as IEvent;
			if (!TriggerEvent(obj))
			{
				Debug.Log((object)("Error when processing event: " + obj.GetName()));
			}
			if (LimitQueueProcesing)
			{
				num += Time.deltaTime;
			}
		}
	}

	public void OnApplicationQuit()
	{
		m_listenerTable.Clear();
		m_eventQueue.Clear();
	}
}
