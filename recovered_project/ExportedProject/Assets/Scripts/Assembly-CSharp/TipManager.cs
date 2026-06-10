using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class TipManager : MonoSingleton<TipManager>
{
	public UILabel tipLabel;

	public List<string> tips;

	private void Start()
	{
		tipLabel.text = tips[UnityEngine.Random.Range(0, tips.Count)];
	}
}
