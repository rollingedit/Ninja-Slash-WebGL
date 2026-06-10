using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class BackGroundConcept
{
	[SerializeField]
	public ConceptType type;

	[SerializeField]
	public GameObject startBlock;

	[SerializeField]
	public List<GameObject> easyBlockList;

	[SerializeField]
	public List<GameObject> blockList;

	[SerializeField]
	public GameObject gateBlock;

	[SerializeField]
	public int conceptBlockCount;

	[SerializeField]
	public List<SkyboxFogColor> skyboxFogColor;

	[SerializeField]
	private bool sakuraEnable;
}
