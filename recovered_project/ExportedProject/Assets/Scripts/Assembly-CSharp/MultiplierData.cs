using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MultiplierData
{
	public int lowerLimit;

	public int upperLimit;

	public int curValue;

	public float guageValue
	{
		get
		{
			return Mathf.InverseLerp((float)lowerLimit, (float)upperLimit, (float)curValue);
		}
	}

	public MultiplierData(int lowerLimitIn, int upperLimitIn, int curValueIn)
	{
		lowerLimit = lowerLimitIn;
		upperLimit = upperLimitIn;
		curValue = curValueIn;
	}
}
