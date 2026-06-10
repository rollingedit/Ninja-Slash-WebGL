using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MouseMove : MonoBehaviour
{
	[SerializeField]
	private float _sensitivity = 0.5f;

	private Vector3 _originalPos;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_originalPos = ((Component)this).transform.position;
	}

	private void Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.x /= (float)Screen.width;
		mousePosition.y /= (float)Screen.height;
		mousePosition.x -= 0.5f;
		mousePosition.y -= 0.5f;
		mousePosition *= 2f * _sensitivity;
		((Component)this).transform.position = _originalPos + mousePosition;
	}
}
