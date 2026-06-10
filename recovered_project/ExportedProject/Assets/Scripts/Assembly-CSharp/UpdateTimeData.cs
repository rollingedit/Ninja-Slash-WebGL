using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class UpdateTimeData
{
	public UpdateTimeTarget target;

	public float curValue;

	public float maxValue;

	public float guageValue
	{
		get
		{
			return Mathf.InverseLerp(0f, maxValue, curValue);
		}
	}

	public UpdateTimeData(float curValueIn, float maxValueIn, UpdateTimeTarget targetIn)
	{
		curValue = curValueIn;
		maxValue = maxValueIn;
		target = targetIn;
	}
}
