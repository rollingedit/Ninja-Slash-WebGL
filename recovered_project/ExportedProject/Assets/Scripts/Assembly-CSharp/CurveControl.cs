using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CurveControl : MonoBehaviour
{
	public Vector2 offset;

	public Material[] materials;

	private void SetOffset(Vector2 offsetIn)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Material[] array = materials;
		foreach (Material val in array)
		{
			val.SetVector("_QOffset", (Vector4)(offsetIn));
		}
	}

	private void Start()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		SetOffset(offset);
	}

	private void OnApplicationQuit()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		SetOffset(Vector2.zero);
	}
}
