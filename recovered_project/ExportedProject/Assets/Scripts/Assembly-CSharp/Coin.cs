using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Coin : MonoBehaviour
{
	private float magnetMovingSpeed = 70f;

	private Vector3 originalPos;

	private bool isMoving;

	private Transform targetTransform;

	private Transform myTransform;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		myTransform = ((Component)this).transform;
		originalPos = myTransform.localPosition;
	}

	private void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (isMoving)
		{
			Vector3 val = targetTransform.position - myTransform.position;
			Vector3 normalized = val.normalized;
			Transform obj = myTransform;
			obj.position += magnetMovingSpeed * normalized * Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		((Component)collider).gameObject.SendMessage("OnEnterCoin", (object)((Component)this).gameObject, (SendMessageOptions)1);
	}

	private void OnGetByPlayer()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		isMoving = false;
		myTransform.localPosition = originalPos;
		((Component)myTransform).gameObject.SetActive(false);
	}

	private void OnEnterMagnetRange()
	{
		if (!isMoving)
		{
			targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
			isMoving = true;
		}
	}

	private void OnPutObjectIntoPool()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		isMoving = false;
		myTransform.localPosition = originalPos;
	}
}
