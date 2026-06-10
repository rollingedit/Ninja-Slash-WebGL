using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CameraMotor : MonoBehaviour
{
	public float cameraHeight;

	public float cameraDistance;

	public float cameraRotateDamping;

	public float cameraDistanceDamping;

	public Vector3 cameraLookAtFix = new Vector3(0f, 5f, 0f);

	public Vector3 cameraStartPos;

	private GameObject cameraTarget;

	private bool isEnabled;

	private float currentWantedHeight;

	private Transform cameraTransform;

	private Transform characterTransform;

	private Transform targetTransform;

	private Vector3 cameraOriginalPos;

	private Quaternion cameraOriginalRot;

	private void Start()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		cameraTransform = ((Component)this).transform;
		cameraOriginalPos = ((Component)this).transform.position;
		cameraOriginalRot = ((Component)this).transform.rotation;
	}

	private void SetTarget(GameObject targetIn)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		characterTransform = targetIn.transform;
		if ((Object)(object)cameraTarget == (Object)null)
		{
			cameraTarget = new GameObject("Camera Target");
			cameraTarget.transform.position = targetIn.transform.position;
			cameraTarget.transform.rotation = targetIn.transform.rotation;
			cameraTarget.transform.parent = targetIn.transform;
			targetTransform = cameraTarget.transform;
		}
		currentWantedHeight = cameraLookAtFix.y;
	}

	private void LateUpdate()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		if (isEnabled && !((Object)(object)cameraTarget == (Object)null))
		{
			targetTransform.position = new Vector3(characterTransform.position.x * 0.85f, currentWantedHeight, characterTransform.position.z);
			Vector3 val = targetTransform.position + new Vector3(0f, cameraHeight, 0f - cameraDistance);
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, val, cameraDistanceDamping * Time.deltaTime);
			if (targetTransform.position.z - cameraDistance >= cameraTransform.position.z)
			{
				cameraTransform.position = new Vector3(targetTransform.position.x, cameraTransform.position.y, targetTransform.position.z - cameraDistance);
			}
			else
			{
				cameraTransform.position = new Vector3(targetTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
			}
			Quaternion val2 = Quaternion.LookRotation(targetTransform.position - cameraTransform.position, targetTransform.up);
			cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, val2, cameraRotateDamping * Time.deltaTime);
		}
	}

	private void OnGameStart()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		cameraTransform.position = cameraStartPos;
		isEnabled = true;
	}

	private void OnHome()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		isEnabled = false;
		cameraTransform.position = cameraOriginalPos;
		cameraTransform.rotation = cameraOriginalRot;
	}

	private void OnGameOver()
	{
		isEnabled = false;
	}

	private void AdjustHeight()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		currentWantedHeight = (characterTransform.position + cameraLookAtFix).y;
	}

	private void OnBounce()
	{
		AdjustHeight();
	}

	private void OnRestart()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		cameraTransform.position = cameraOriginalPos;
		isEnabled = true;
	}

	private void OnShieldUsed()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		cameraTransform.position = cameraOriginalPos;
		cameraTransform.rotation = cameraOriginalRot;
		currentWantedHeight = cameraLookAtFix.y;
	}

	private void OnSpawnObject(GameObject objectIn)
	{
		if (objectIn.tag == "Player")
		{
			SetTarget(objectIn);
		}
	}
}
