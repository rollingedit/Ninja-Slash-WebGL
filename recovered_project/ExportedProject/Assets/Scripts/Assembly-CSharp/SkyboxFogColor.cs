using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[Serializable]
public class SkyboxFogColor
{
	[SerializeField]
	public Material skybox;

	[SerializeField]
	public Color fogColor;

	[SerializeField]
	public Color ambientColor;

	[SerializeField]
	public Color zombientColor;
}
