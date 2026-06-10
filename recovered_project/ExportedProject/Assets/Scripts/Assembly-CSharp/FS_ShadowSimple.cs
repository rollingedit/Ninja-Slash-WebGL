using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("Fast Shadows/Simple Shadow")]
public class FS_ShadowSimple : MonoBehaviour
{
	[HideInInspector]
	public float maxProjectionDistance = 100f;

	[HideInInspector]
	public float girth = 1f;

	[HideInInspector]
	public float shadowHoverHeight = 0.2f;

	public LayerMask layerMask = -1;

	[HideInInspector]
	public Material shadowMaterial;

	[HideInInspector]
	public bool isStatic;

	[HideInInspector]
	public bool useLightSource;

	[HideInInspector]
	public GameObject lightSource;

	[HideInInspector]
	public Vector3 lightDirection = new Vector3(0f, -1f, 0f);

	[HideInInspector]
	public bool isPerspectiveProjection;

	[HideInInspector]
	public Rect uvs = new Rect(0f, 0f, 1f, 1f);

	private float _girth;

	private Vector3 _lightDirection = Vector3.zero;

	private bool isGoodPlaneIntersect;

	private Color gizmoColor = Color.white;

	private Vector3[] _corners = (Vector3[])(object)new Vector3[4];

	private Color _color = new Color(1f, 1f, 1f, 0f);

	private Vector3 _normal;

	private GameObject[] cornerGOs = (GameObject[])(object)new GameObject[4];

	private GameObject shadowCaster;

	private Plane shadowPlane = default(Plane);

	private FS_MeshKey meshKey;

	public Vector3[] corners
	{
		get
		{
			return _corners;
		}
	}

	public Color color
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _color;
		}
	}

	public Vector3 normal
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _normal;
		}
	}

	private void Awake()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if ((Object)(object)shadowMaterial == (Object)null)
		{
			shadowMaterial = (Material)Resources.Load("FS_ShadowMaterial");
			if ((Object)(object)shadowMaterial == (Object)null)
			{
				Debug.LogWarning((object)("Shadow Material is not set for " + ((Object)this).name));
			}
		}
		if (isStatic)
		{
			CalculateShadowGeometry();
		}
	}

	private void CalculateShadowGeometry()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)shadowMaterial == (Object)null)
		{
			return;
		}
		if (useLightSource && (Object)(object)lightSource == (Object)null)
		{
			useLightSource = false;
			Debug.LogWarning((object)"No light source object given using light direction vector.");
		}
		if (useLightSource)
		{
			Vector3 val = ((Component)this).transform.position - lightSource.transform.position;
			float magnitude = val.magnitude;
			if (magnitude == 0f)
			{
				return;
			}
			lightDirection = val / magnitude;
		}
		else if (lightDirection != _lightDirection || lightDirection == Vector3.zero)
		{
			if (lightDirection == Vector3.zero)
			{
				Debug.LogWarning((object)"Light Direction vector cannot be zero. assuming -y.");
				lightDirection = -Vector3.up;
			}
			lightDirection.Normalize();
			_lightDirection = lightDirection;
		}
		if ((Object)(object)shadowCaster == (Object)null || girth != _girth)
		{
			if ((Object)(object)shadowCaster == (Object)null)
			{
				shadowCaster = new GameObject("shadowSimple");
				cornerGOs = (GameObject[])(object)new GameObject[4];
				for (int i = 0; i < 4; i++)
				{
					GameObject[] array = cornerGOs;
					int num = i;
					GameObject val2 = new GameObject(string.Concat((object)"c", (object)i));
					GameObject val3 = val2;
					array[num] = val2;
					GameObject val4 = val3;
					val4.transform.parent = shadowCaster.transform;
				}
				shadowCaster.transform.parent = ((Component)this).transform;
				shadowCaster.transform.localPosition = Vector3.zero;
				shadowCaster.transform.localRotation = Quaternion.identity;
				shadowCaster.transform.localScale = Vector3.one;
			}
			Vector3 val5 = ((!(Mathf.Abs(Vector3.Dot(((Component)this).transform.forward, lightDirection)) < 0.9f)) ? (((Component)this).transform.up - Vector3.Dot(((Component)this).transform.up, lightDirection) * lightDirection) : (((Component)this).transform.forward - Vector3.Dot(((Component)this).transform.forward, lightDirection) * lightDirection));
			shadowCaster.transform.rotation = Quaternion.LookRotation(val5, -lightDirection);
			cornerGOs[0].transform.position = shadowCaster.transform.position + girth * (shadowCaster.transform.forward - shadowCaster.transform.right);
			cornerGOs[1].transform.position = shadowCaster.transform.position + girth * (shadowCaster.transform.forward + shadowCaster.transform.right);
			cornerGOs[2].transform.position = shadowCaster.transform.position + girth * (-shadowCaster.transform.forward + shadowCaster.transform.right);
			cornerGOs[3].transform.position = shadowCaster.transform.position + girth * (-shadowCaster.transform.forward - shadowCaster.transform.right);
			_girth = girth;
		}
		Transform transform = shadowCaster.transform;
		Ray val6 = default(Ray);
		val6 = new Ray(transform.position, lightDirection);
		RaycastHit val7 = default(RaycastHit);
		if (maxProjectionDistance > 0f && Physics.Raycast(val6, out val7, maxProjectionDistance, (layerMask).value))
		{
			Vector3 val5;
			if (Mathf.Abs(Vector3.Dot(((Component)this).transform.forward, lightDirection)) < 0.9f)
			{
				val5 = ((Component)this).transform.forward - Vector3.Dot(((Component)this).transform.forward, lightDirection) * lightDirection;
			}
			else
			{
				Debug.Log((object)lightDirection);
				val5 = ((Component)this).transform.up - Vector3.Dot(((Component)this).transform.up, lightDirection) * lightDirection;
			}
			shadowCaster.transform.rotation = Quaternion.Lerp(shadowCaster.transform.rotation, Quaternion.LookRotation(val5, -lightDirection), 0.1f);
			float num2 = val7.distance - shadowHoverHeight;
			float num3 = 1f - num2 / maxProjectionDistance;
			if (num3 < 0f)
			{
				return;
			}
			num3 = Mathf.Clamp01(num3);
			_color.a = num3;
			_normal = val7.normal;
			Vector3 val8 = val7.point - shadowHoverHeight * lightDirection;
			shadowPlane.SetNormalAndPosition(_normal, val8);
			isGoodPlaneIntersect = true;
			float num4 = 0f;
			float num5 = default(float);
			if (useLightSource && isPerspectiveProjection)
			{
				val6.origin = lightSource.transform.position;
				Vector3 val9 = cornerGOs[0].transform.position - lightSource.transform.position;
				num4 = val9.magnitude;
				val6.direction = val9 / num4;
				isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(val6, out num5);
				_corners[0] = val6.origin + val6.direction * num5;
				val9 = cornerGOs[1].transform.position - lightSource.transform.position;
				val6.direction = val9 / num4;
				isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(val6, out num5);
				_corners[1] = val6.origin + val6.direction * num5;
				val9 = cornerGOs[2].transform.position - lightSource.transform.position;
				val6.direction = val9 / num4;
				isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(val6, out num5);
				_corners[2] = val6.origin + val6.direction * num5;
				val9 = cornerGOs[3].transform.position - lightSource.transform.position;
				val6.direction = val9 / num4;
				isGoodPlaneIntersect = isGoodPlaneIntersect && shadowPlane.Raycast(val6, out num5);
				_corners[3] = val6.origin + val6.direction * num5;
			}
			else
			{
				val6.origin = cornerGOs[0].transform.position;
				isGoodPlaneIntersect = shadowPlane.Raycast(val6, out num5);
				if (!isGoodPlaneIntersect && num5 == 0f)
				{
					return;
				}
				isGoodPlaneIntersect = true;
				_corners[0] = val6.origin + val6.direction * num5;
				val6.origin = cornerGOs[1].transform.position;
				isGoodPlaneIntersect = shadowPlane.Raycast(val6, out num5);
				if (!isGoodPlaneIntersect && num5 == 0f)
				{
					return;
				}
				isGoodPlaneIntersect = true;
				_corners[1] = val6.origin + val6.direction * num5;
				val6.origin = cornerGOs[2].transform.position;
				isGoodPlaneIntersect = shadowPlane.Raycast(val6, out num5);
				if (!isGoodPlaneIntersect && num5 == 0f)
				{
					return;
				}
				isGoodPlaneIntersect = true;
				_corners[2] = val6.origin + val6.direction * num5;
				val6.origin = cornerGOs[3].transform.position;
				isGoodPlaneIntersect = shadowPlane.Raycast(val6, out num5);
				if (!isGoodPlaneIntersect && num5 == 0f)
				{
					return;
				}
				isGoodPlaneIntersect = true;
				_corners[3] = val6.origin + val6.direction * num5;
			}
			if (isGoodPlaneIntersect)
			{
				if (meshKey == null || (Object)(object)meshKey.mat != (Object)(object)shadowMaterial || meshKey.isStatic != isStatic)
				{
					meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				}
				FS_ShadowManager.Manager().registerGeometry(this, meshKey);
				gizmoColor = Color.white;
			}
			else
			{
				gizmoColor = Color.magenta;
			}
		}
		else
		{
			isGoodPlaneIntersect = false;
			gizmoColor = Color.red;
		}
	}

	private void Update()
	{
		if (!isStatic)
		{
			CalculateShadowGeometry();
		}
	}

	private void OnDrawGizmos()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)shadowCaster != (Object)null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.up);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.forward);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.right);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(shadowCaster.transform.position, ((Component)this).transform.forward);
			Gizmos.color = gizmoColor;
			if (isGoodPlaneIntersect)
			{
				Gizmos.DrawLine(cornerGOs[0].transform.position, corners[0]);
				Gizmos.DrawLine(cornerGOs[1].transform.position, corners[1]);
				Gizmos.DrawLine(cornerGOs[2].transform.position, corners[2]);
				Gizmos.DrawLine(cornerGOs[3].transform.position, corners[3]);
				Gizmos.DrawLine(cornerGOs[0].transform.position, cornerGOs[1].transform.position);
				Gizmos.DrawLine(cornerGOs[1].transform.position, cornerGOs[2].transform.position);
				Gizmos.DrawLine(cornerGOs[2].transform.position, cornerGOs[3].transform.position);
				Gizmos.DrawLine(cornerGOs[3].transform.position, cornerGOs[0].transform.position);
				Gizmos.DrawLine(corners[0], corners[1]);
				Gizmos.DrawLine(corners[1], corners[2]);
				Gizmos.DrawLine(corners[2], corners[3]);
				Gizmos.DrawLine(corners[3], corners[0]);
			}
		}
	}
}
