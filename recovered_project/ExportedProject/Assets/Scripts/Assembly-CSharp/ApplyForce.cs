using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ApplyForce : MonoBehaviour
{
	public float forceMultiplier;

	public float originalDirectionWeight = 5f;

	public float randomDirectionWeight = 2f;

	private void OnApplyForce(Vector3 velocity)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		float magnitude = velocity.magnitude;
		foreach (Transform item in ((Component)this).transform)
		{
			Transform val = item;
			Vector3 val2 = velocity.normalized * originalDirectionWeight + Random.onUnitSphere * randomDirectionWeight;
			Vector3 normalized = val2.normalized;
			((Component)val).gameObject.GetComponent<Rigidbody>().AddForce(normalized * forceMultiplier * magnitude, (ForceMode)0);
		}
	}
}
