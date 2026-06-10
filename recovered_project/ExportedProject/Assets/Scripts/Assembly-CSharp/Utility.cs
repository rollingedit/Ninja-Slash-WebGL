using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Utility
{
	public static void SetActiveRecursively(GameObject go, bool isTrue)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		go.SetActive(isTrue);
		foreach (Transform item in go.transform)
		{
			Transform val = item;
			SetActiveRecursively(((Component)val).gameObject, isTrue);
		}
	}

	public static bool IsContainingComponent(GameObject go, global::System.Type type)
	{
		if ((Object)(object)go.GetComponent(type) != (Object)null)
		{
			return true;
		}
		return false;
	}

	public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		return (1f - t) * (1f - t) * Start + 2f * t * (1f - t) * Control + t * t * End;
	}

	public static void PlaySound(AudioSource audio, AudioClip clipIn)
	{
		audio.PlayOneShot(clipIn);
	}

	public static bool IsMobile()
	{
		bool flag = false;
		return false;
	}

	public static bool IsGoodPerformance()
	{
		return true;
	}

	public static void GetRigidbodyRecursively(Transform transformIn, List<Transform> childList)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		if ((Object)(object)((Component)transformIn).GetComponent<Rigidbody>() != (Object)null)
		{
			childList.Add(transformIn);
		}
		foreach (Transform item in transformIn)
		{
			Transform transformIn2 = item;
			GetRigidbodyRecursively(transformIn2, childList);
		}
	}
}
