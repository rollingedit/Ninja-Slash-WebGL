using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T m_Instance;

	public static T instance
	{
		get
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_Instance == (Object)null)
			{
				m_Instance = Object.FindObjectOfType(typeof(T)) as T;
				if ((Object)(object)m_Instance == (Object)null)
				{
					Debug.LogWarning((object)("No instance of " + typeof(T).ToString() + ", a temporary one is created."));
					m_Instance = new GameObject("Temp Instance of " + typeof(T).ToString(), new global::System.Type[1] { typeof(T) }).GetComponent<T>();
					if ((Object)(object)m_Instance == (Object)null)
					{
						Debug.LogError((object)("Problem during the creation of " + typeof(T).ToString()));
					}
				}
				m_Instance.Init();
			}
			return m_Instance;
		}
	}

	private void Awake()
	{
		if ((Object)(object)m_Instance == (Object)null)
		{
			m_Instance = this as T;
			m_Instance.Init();
		}
	}

	public virtual void Init()
	{
	}

	private void OnApplicationQuit()
	{
		m_Instance = (T)null;
	}
}
