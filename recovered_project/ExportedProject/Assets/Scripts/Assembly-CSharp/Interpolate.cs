using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Interpolate
{
	public enum EaseType
	{
		Linear = 0,
		EaseInQuad = 1,
		EaseOutQuad = 2,
		EaseInOutQuad = 3,
		EaseInCubic = 4,
		EaseOutCubic = 5,
		EaseInOutCubic = 6,
		EaseInQuart = 7,
		EaseOutQuart = 8,
		EaseInOutQuart = 9,
		EaseInQuint = 10,
		EaseOutQuint = 11,
		EaseInOutQuint = 12,
		EaseInSine = 13,
		EaseOutSine = 14,
		EaseInOutSine = 15,
		EaseInExpo = 16,
		EaseOutExpo = 17,
		EaseInOutExpo = 18,
		EaseInCirc = 19,
		EaseOutCirc = 20,
		EaseInOutCirc = 21
	}

	public delegate Vector3 ToVector3<T>(T v);

	public delegate float Function(float a, float b, float c, float d);

	private static Vector3 Identity(Vector3 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return v;
	}

	private static Vector3 TransformDotPosition(Transform t)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return t.position;
	}

	[DebuggerHidden]
	private static global::System.Collections.Generic.IEnumerable<float> NewTimer(float duration)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return elapsedTime;
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= duration)
			{
				yield return elapsedTime;
			}
		}
	}

	[DebuggerHidden]
	private static global::System.Collections.Generic.IEnumerable<float> NewCounter(int start, int end, int step)
	{
		for (int i = start; i <= end; i += step)
		{
			yield return i;
		}
	}

	public static global::System.Collections.IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, float duration)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerable<float> driver = NewTimer(duration);
		return NewEase(ease, start, end, duration, driver);
	}

	public static global::System.Collections.IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, int slices)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerable<float> driver = NewCounter(0, slices + 1, 1);
		return NewEase(ease, start, end, slices + 1, driver);
	}

	[DebuggerHidden]
	private static global::System.Collections.IEnumerator NewEase(Function ease, Vector3 start, Vector3 end, float total, global::System.Collections.Generic.IEnumerable<float> driver)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Vector3 distance = end - start;
		global::System.Collections.Generic.IEnumerator<float> enumerator = driver.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				float i = enumerator.Current;
				yield return Ease(ease, start, distance, i, total);
			}
		}
		finally
		{
			if (enumerator != null)
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}

	private static Vector3 Ease(Function ease, Vector3 start, Vector3 distance, float elapsedTime, float duration)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		start.x = ease(start.x, distance.x, elapsedTime, duration);
		start.y = ease(start.y, distance.y, elapsedTime, duration);
		start.z = ease(start.z, distance.z, elapsedTime, duration);
		return start;
	}

	public static Function Ease(EaseType type)
	{
		Function result = null;
		switch (type)
		{
		case EaseType.Linear:
			result = Linear;
			break;
		case EaseType.EaseInQuad:
			result = EaseInQuad;
			break;
		case EaseType.EaseOutQuad:
			result = EaseOutQuad;
			break;
		case EaseType.EaseInOutQuad:
			result = EaseInOutQuad;
			break;
		case EaseType.EaseInCubic:
			result = EaseInCubic;
			break;
		case EaseType.EaseOutCubic:
			result = EaseOutCubic;
			break;
		case EaseType.EaseInOutCubic:
			result = EaseInOutCubic;
			break;
		case EaseType.EaseInQuart:
			result = EaseInQuart;
			break;
		case EaseType.EaseOutQuart:
			result = EaseOutQuart;
			break;
		case EaseType.EaseInOutQuart:
			result = EaseInOutQuart;
			break;
		case EaseType.EaseInQuint:
			result = EaseInQuint;
			break;
		case EaseType.EaseOutQuint:
			result = EaseOutQuint;
			break;
		case EaseType.EaseInOutQuint:
			result = EaseInOutQuint;
			break;
		case EaseType.EaseInSine:
			result = EaseInSine;
			break;
		case EaseType.EaseOutSine:
			result = EaseOutSine;
			break;
		case EaseType.EaseInOutSine:
			result = EaseInOutSine;
			break;
		case EaseType.EaseInExpo:
			result = EaseInExpo;
			break;
		case EaseType.EaseOutExpo:
			result = EaseOutExpo;
			break;
		case EaseType.EaseInOutExpo:
			result = EaseInOutExpo;
			break;
		case EaseType.EaseInCirc:
			result = EaseInCirc;
			break;
		case EaseType.EaseOutCirc:
			result = EaseOutCirc;
			break;
		case EaseType.EaseInOutCirc:
			result = EaseInOutCirc;
			break;
		}
		return result;
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewBezier(Function ease, Transform[] nodes, float duration)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = NewTimer(duration);
		return NewBezier<Transform>(ease, (global::System.Collections.IList)(object)nodes, TransformDotPosition, duration, steps);
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewBezier(Function ease, Transform[] nodes, int slices)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = NewCounter(0, slices + 1, 1);
		return NewBezier<Transform>(ease, (global::System.Collections.IList)(object)nodes, TransformDotPosition, slices + 1, steps);
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewBezier(Function ease, Vector3[] points, float duration)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = NewTimer(duration);
		return NewBezier<Vector3>(ease, (global::System.Collections.IList)(object)points, Identity, duration, steps);
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewBezier(Function ease, Vector3[] points, int slices)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = NewCounter(0, slices + 1, 1);
		return NewBezier<Vector3>(ease, (global::System.Collections.IList)(object)points, Identity, slices + 1, steps);
	}

	[DebuggerHidden]
	private static global::System.Collections.Generic.IEnumerable<Vector3> NewBezier<T>(Function ease, global::System.Collections.IList nodes, ToVector3<T> toVector3, float maxStep, global::System.Collections.Generic.IEnumerable<float> steps)
	{
		if (((global::System.Collections.ICollection)nodes).Count < 2)
		{
			yield break;
		}
		Vector3[] points = (Vector3[])(object)new Vector3[((global::System.Collections.ICollection)nodes).Count];
		global::System.Collections.Generic.IEnumerator<float> enumerator = steps.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				float step = enumerator.Current;
				for (int i = 0; i < ((global::System.Collections.ICollection)nodes).Count; i++)
				{
					points[i] = toVector3((T)nodes[i]);
				}
				yield return Bezier(ease, points, step, maxStep);
			}
		}
		finally
		{
			if (enumerator != null)
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}
	}

	private static Vector3 Bezier(Function ease, Vector3[] points, float elapsedTime, float duration)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		for (int num = points.Length - 1; num > 0; num--)
		{
			for (int i = 0; i < num; i++)
			{
				points[i].x = ease(points[i].x, points[i + 1].x - points[i].x, elapsedTime, duration);
				points[i].y = ease(points[i].y, points[i + 1].y - points[i].y, elapsedTime, duration);
				points[i].z = ease(points[i].z, points[i + 1].z - points[i].z, elapsedTime, duration);
			}
		}
		return points[0];
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewCatmullRom(Transform[] nodes, int slices, bool loop)
	{
		return NewCatmullRom<Transform>((global::System.Collections.IList)(object)nodes, TransformDotPosition, slices, loop);
	}

	public static global::System.Collections.Generic.IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
	{
		return NewCatmullRom<Vector3>((global::System.Collections.IList)(object)points, Identity, slices, loop);
	}

	[DebuggerHidden]
	private static global::System.Collections.Generic.IEnumerable<Vector3> NewCatmullRom<T>(global::System.Collections.IList nodes, ToVector3<T> toVector3, int slices, bool loop)
	{
		if (((global::System.Collections.ICollection)nodes).Count < 2)
		{
			yield break;
		}
		yield return toVector3((T)nodes[0]);
		int last = ((global::System.Collections.ICollection)nodes).Count - 1;
		for (int current = 0; loop || current < last; current++)
		{
			if (loop && current > last)
			{
				current = 0;
			}
			int previous = ((current != 0) ? (current - 1) : ((!loop) ? current : last));
			int start = current;
			int end = ((current != last) ? (current + 1) : ((!loop) ? current : 0));
			int next = ((end != last) ? (end + 1) : ((!loop) ? end : 0));
			int stepCount = slices + 1;
			for (int step = 1; step <= stepCount; step++)
			{
				yield return CatmullRom(toVector3((T)nodes[previous]), toVector3((T)nodes[start]), toVector3((T)nodes[end]), toVector3((T)nodes[next]), step, stepCount);
			}
		}
	}

	private static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		float num = elapsedTime / duration;
		float num2 = num * num;
		float num3 = num2 * num;
		return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
	}

	private static float Linear(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (elapsedTime / duration) + start;
	}

	private static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return (0f - distance) * elapsedTime * (elapsedTime - 2f) + start;
	}

	private static float EaseInOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 1f;
		return (0f - distance) / 2f * (elapsedTime * (elapsedTime - 2f) - 1f) + start;
	}

	private static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	private static float EaseInOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	private static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return (0f - distance) * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1f) + start;
	}

	private static float EaseInOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return (0f - distance) / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2f) + start;
	}

	private static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	private static float EaseInOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	private static float EaseInSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return (0f - distance) * Mathf.Cos(elapsedTime / duration * ((float)Math.PI / 2f)) + distance + start;
	}

	private static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Sin(elapsedTime / duration * ((float)Math.PI / 2f)) + start;
	}

	private static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return (0f - distance) / 2f * (Mathf.Cos((float)Math.PI * elapsedTime / duration) - 1f) + start;
	}

	private static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Pow(2f, 10f * (elapsedTime / duration - 1f)) + start;
	}

	private static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (0f - Mathf.Pow(2f, -10f * elapsedTime / duration) + 1f) + start;
	}

	private static float EaseInOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * Mathf.Pow(2f, 10f * (elapsedTime - 1f)) + start;
		}
		elapsedTime -= 1f;
		return distance / 2f * (0f - Mathf.Pow(2f, -10f * elapsedTime) + 2f) + start;
	}

	private static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		return (0f - distance) * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
	}

	private static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * Mathf.Sqrt(1f - elapsedTime * elapsedTime) + start;
	}

	private static float EaseInOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((!(elapsedTime > duration)) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return (0f - distance) / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) + 1f) + start;
	}
}
