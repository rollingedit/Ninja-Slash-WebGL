using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CubeRotator : MonoBehaviour
{
	public float speed = 15f;

	private Transform cube;

	private bool shouldRotate = true;

	private void Start()
	{
		cube = ((Component)this).GetComponent<Transform>();
	}

	private void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (shouldRotate)
		{
			cube.Rotate(Vector3.forward, Time.deltaTime * speed);
		}
	}

	public void togglePauseRotation()
	{
		Debug.Log((object)"toggle pause");
		shouldRotate = !shouldRotate;
	}
}
