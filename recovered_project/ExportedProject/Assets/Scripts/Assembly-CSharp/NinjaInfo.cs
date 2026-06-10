using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class NinjaInfo : MonoSingleton<NinjaInfo>
{
	public List<Ninja> ninjaList;

	public Ninja GetNinjaByName(string nameIn)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		var enumerator = ninjaList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Ninja current = enumerator.Current;
				if (current.ninjaName == nameIn)
				{
					return current;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		Debug.LogError((object)"No Matching Ninja!!");
		return null;
	}

	public Ninja GetCurrentSelectedNinja()
	{
		if (ninjaList == null || ninjaList.Count == 0)
		{
			return null;
		}
		MonoSingleton<UserData>.instance.SelectedNinjaIndex = Mathf.Clamp(MonoSingleton<UserData>.instance.SelectedNinjaIndex, 0, ninjaList.Count - 1);
		return ninjaList[MonoSingleton<UserData>.instance.SelectedNinjaIndex];
	}
}
