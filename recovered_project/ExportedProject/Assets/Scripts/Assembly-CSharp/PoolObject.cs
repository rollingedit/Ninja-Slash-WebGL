using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class PoolObject : MonoBehaviour
{
	public string PrefabName;

	public EventHandler Spawned;

	public EventHandler Despawned;

	private bool m_isSpawned;

	private bool HasDespawnTimer;

	private float m_timeLastSpawned = -1f;

	private float m_timeLastDespawned = -1f;

	private float DespawnTimerInitialized = -1f;

	private float DespawnDelay = -1f;

	public bool IsSpawned
	{
		get
		{
			return m_isSpawned;
		}
		private set
		{
			m_isSpawned = value;
		}
	}

	public float TimeLastSpawned
	{
		get
		{
			return m_timeLastSpawned;
		}
		private set
		{
			m_timeLastSpawned = value;
		}
	}

	public float TimeLastDespawned
	{
		get
		{
			return m_timeLastDespawned;
		}
		private set
		{
			m_timeLastDespawned = value;
		}
	}

	private float Age
	{
		get
		{
			if (!IsSpawned)
			{
				return -1f;
			}
			return Time.time - TimeLastSpawned;
		}
	}

	private float AgeAsScalar
	{
		get
		{
			if (!IsSpawned)
			{
				return -1f;
			}
			if (DespawnDelay <= 0f)
			{
				return 1f;
			}
			return (Time.time - DespawnTimerInitialized) / DespawnDelay;
		}
	}

	private void Awake()
	{
		((Component)this).transform.parent = ((Component)PoolManager.Instance).gameObject.transform;
		SetActive(false);
	}

	private void SetActive(bool active)
	{
		((Component)this).gameObject.SetActive(active);
	}

	public void OnSpawn(bool isActivateRecursively)
	{
		if (!IsSpawned)
		{
			IsSpawned = true;
			TimeLastSpawned = Time.time;
			((Component)this).transform.parent = null;
			if (isActivateRecursively)
			{
				Utility.SetActiveRecursively(((Component)this).gameObject, true);
			}
			else
			{
				SetActive(true);
			}
			if (Spawned != null)
			{
				Spawned.Invoke((object)this, (EventArgs)null);
			}
		}
	}

	public void OnDespawn()
	{
		if (IsSpawned)
		{
			IsSpawned = false;
			HasDespawnTimer = false;
			TimeLastDespawned = Time.time;
			DespawnTimerInitialized = -1f;
			DespawnDelay = -1f;
			((MonoBehaviour)this).StopAllCoroutines();
			if ((Object)(object)((Component)this).transform.parent != (Object)null)
			{
				((Component)((Component)this).transform.parent).SendMessage("OnObjectDespawned", (SendMessageOptions)1);
			}
			SetActive(false);
			((Component)this).transform.parent = ((Component)PoolManager.Instance).gameObject.transform;
			if (Despawned != null)
			{
				Despawned.Invoke((object)this, (EventArgs)null);
			}
		}
	}

	public void Despawn()
	{
		PoolManager.Despawn(((Component)this).gameObject);
	}

	public void DespawnAfterSeconds(float delay)
	{
		if (IsSpawned)
		{
			DespawnTimerInitialized = Time.time;
			DespawnDelay = delay - Time.deltaTime;
			if (!HasDespawnTimer)
			{
				((MonoBehaviour)this).StartCoroutine(CRDespawnAfterSeconds(delay));
			}
		}
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator CRDespawnAfterSeconds(float delay)
	{
		HasDespawnTimer = true;
		while (Time.time < DespawnTimerInitialized + DespawnDelay)
		{
			yield return null;
		}
		Despawn();
	}
}
