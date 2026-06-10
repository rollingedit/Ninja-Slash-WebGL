using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class AttachTextureData
{
	public GameObject texture;

	public float time;

	public AttachTextureData(GameObject textureIn, float timeIn)
	{
		texture = textureIn;
		time = timeIn;
	}
}
