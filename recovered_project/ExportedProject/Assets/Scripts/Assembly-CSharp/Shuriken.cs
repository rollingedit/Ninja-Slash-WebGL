using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Shuriken : MonoBehaviour
{
	public float relativeSpeed;

	public float livingTime;

	public TrailRenderer trailRenderer;

	private float speed;

	private bool isTargetLockedOn;

	private Vector3 originPos;

	private Vector3 targetPos;

	private Vector3 controlPos;

	private float distance;

	private float bezierTime;

	private Vector3 moveDir;

	private Transform myTransform;

	private void Start()
	{
		myTransform = ((Component)this).transform;
		if (!Utility.IsGoodPerformance())
		{
			((Renderer)trailRenderer).enabled = false;
		}
	}

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (isTargetLockedOn)
		{
			Vector3 val = Utility.Bezier2(originPos, controlPos, targetPos, bezierTime);
			moveDir = val - myTransform.position;
			myTransform.position = val;
			bezierTime += Time.deltaTime * (speed / distance);
		}
	}

	private void OnTraceTarget(GameObject go)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		speed = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMotor>().forwardSpeed + relativeSpeed;
		originPos = ((Component)this).transform.position;
		targetPos = go.transform.position;
		Vector3 val = Vector3.Cross(targetPos - originPos, Vector3.up);
		Vector3 normalized = val.normalized;
		controlPos = (originPos + targetPos) / 2f + (float)UnityEngine.Random.Range(-10, 10) * normalized;
		Vector3 val2 = ((Component)this).transform.position - go.transform.position;
		distance = val2.magnitude;
		bezierTime = 0f;
		isTargetLockedOn = true;
		((MonoBehaviour)this).StartCoroutine("WaitAndPoolShuriken");
	}

	private void OnEnterZombie(ZombieData data)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		data.zombieObject.BroadcastMessage("OnHitByShuriken", (object)(moveDir.normalized * speed * GetComponent<Rigidbody>().mass), (SendMessageOptions)1);
		PutIntoPool();
	}

	private void OnEnterBamboo(GameObject bamboo)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		bamboo.SendMessage("OnHitByShuriken", (object)(moveDir.normalized * speed * GetComponent<Rigidbody>().mass), (SendMessageOptions)1);
		PutIntoPool();
	}

	private void PutIntoPool()
	{
		((MonoBehaviour)this).StopCoroutine("WaitAndPoolShuriken");
		isTargetLockedOn = false;
		PoolManager.Despawn(((Component)this).gameObject);
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator WaitAndPoolShuriken()
	{
		yield return (object)new WaitForSeconds(livingTime);
		isTargetLockedOn = false;
		PoolManager.Despawn(((Component)this).gameObject);
	}

	private void OnPutObjectIntoPool()
	{
		MonoSingleton<EventManager>.instance.TriggerEvent(new OnShurikenPooledEvent());
	}
}
