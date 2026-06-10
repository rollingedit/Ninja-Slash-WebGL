using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Examples/Drag & Drop Root")]
public class DragDropRoot : MonoBehaviour
{
	public static Transform root;

	private void Awake()
	{
		root = ((Component)this).transform;
	}
}
