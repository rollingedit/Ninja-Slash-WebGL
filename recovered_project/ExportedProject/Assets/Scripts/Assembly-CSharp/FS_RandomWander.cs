using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class FS_RandomWander : MonoBehaviour
{
	public float speed = 0.1f;

	public float directionChangeInterval = 1f;

	public float maxHeadingChange = 30f;

	public float dist;

	private float heading;

	private Vector3 targetRotation;

	private void Awake()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		heading = UnityEngine.Random.Range(0, 360);
		((Component)this).transform.eulerAngles = new Vector3(0f, heading, 0f);
		((MonoBehaviour)this).StartCoroutine(NewHeading());
	}

	private void FixedUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.eulerAngles = Vector3.Slerp(((Component)this).transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		Vector3 val = ((Component)this).transform.TransformDirection(Vector3.forward);
		((Component)this).transform.Translate(val * speed);
		if (((Component)this).transform.position.y < 1f)
		{
			NewHeadingRoutine();
		}
		Vector3 position = ((Component)this).transform.position;
		dist = position.magnitude;
		Vector3 position2 = ((Component)this).transform.position;
		if (position2.magnitude > 70f)
		{
			((Component)this).transform.position = new Vector3(0f, 10f, 0f);
		}
		Vector3 position3 = ((Component)this).transform.position;
		position3.y = 10f;
		((Component)this).transform.position = position3;
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator NewHeading()
	{
		while (true)
		{
			NewHeadingRoutine();
			yield return (object)new WaitForSeconds(directionChangeInterval);
		}
	}

	private void NewHeadingRoutine()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Clamp(heading - maxHeadingChange, 0f, 360f);
		float num2 = Mathf.Clamp(heading + maxHeadingChange, 0f, 360f);
		heading = UnityEngine.Random.Range(num, num2);
		targetRotation = new Vector3(0f, heading, 0f);
	}
}
