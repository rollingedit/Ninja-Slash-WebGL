using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class MeleeWeaponTrail : MonoBehaviour
{
	[Serializable]
	public class Point
	{
		public float timeCreated;

		public Vector3 basePosition;

		public Vector3 tipPosition;
	}

	public float duration;

	[SerializeField]
	private bool _emit = true;

	private bool _use = true;

	[SerializeField]
	private float _emitTime;

	[SerializeField]
	private Material _material;

	[SerializeField]
	private Material _materialOnShield;

	[SerializeField]
	private float _lifeTime = 1f;

	[SerializeField]
	private Color[] _colors;

	[SerializeField]
	private float[] _sizes;

	[SerializeField]
	private float _minVertexDistance = 0.1f;

	[SerializeField]
	private float _maxVertexDistance = 10f;

	private float _minVertexDistanceSqr;

	private float _maxVertexDistanceSqr;

	[SerializeField]
	private float _maxAngle = 3f;

	[SerializeField]
	private bool _autoDestruct;

	[SerializeField]
	private int subdivisions = 4;

	[SerializeField]
	private Transform _base;

	[SerializeField]
	private Transform _tip;

	private List<Point> _points = new List<Point>();

	private List<Point> _smoothedPoints = new List<Point>();

	private GameObject _trailObject;

	private Mesh _trailMesh;

	private Vector3 _lastPosition;

	public bool Emit
	{
		set
		{
			_emit = value;
		}
	}

	public bool Use
	{
		set
		{
			_use = value;
		}
	}

	private void MakeTrail()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		_lastPosition = ((Component)this).transform.position;
		_trailObject = new GameObject("Trail");
		_trailObject.transform.parent = null;
		_trailObject.transform.position = Vector3.zero;
		_trailObject.transform.rotation = Quaternion.identity;
		_trailObject.transform.localScale = Vector3.one;
		_trailObject.AddComponent(typeof(MeshFilter));
		_trailObject.AddComponent(typeof(MeshRenderer));
		_trailObject.GetComponent<Renderer>().material = _material;
		_trailMesh = new Mesh();
		((Object)_trailMesh).name = ((Object)this).name + "TrailMesh";
		_trailObject.GetComponent<MeshFilter>().mesh = _trailMesh;
		_minVertexDistanceSqr = _minVertexDistance * _minVertexDistance;
		_maxVertexDistanceSqr = _maxVertexDistance * _maxVertexDistance;
	}

	private void Update()
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		if (!_use)
		{
			return;
		}
		if (_emit && _emitTime != 0f)
		{
			_emitTime -= Time.deltaTime;
			if (_emitTime == 0f)
			{
				_emitTime = -1f;
			}
			if (_emitTime < 0f)
			{
				_emit = false;
			}
		}
		if (!_emit && _points.Count == 0 && _autoDestruct)
		{
			Object.Destroy((Object)(object)_trailObject);
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		if (((Object)(object)Camera.main == (Object)null))
		{
			return;
		}
		Vector3 val = _lastPosition - ((Component)this).transform.position;
		float sqrMagnitude = val.sqrMagnitude;
		if (_emit)
		{
			if (sqrMagnitude > _minVertexDistanceSqr)
			{
				bool flag = false;
				if (_points.Count < 3)
				{
					flag = true;
				}
				else
				{
					Vector3 val2 = _points[_points.Count - 2].tipPosition - _points[_points.Count - 3].tipPosition;
					Vector3 val3 = _points[_points.Count - 1].tipPosition - _points[_points.Count - 2].tipPosition;
					if (Vector3.Angle(val2, val3) > _maxAngle || sqrMagnitude > _maxVertexDistanceSqr)
					{
						flag = true;
					}
				}
				if (flag)
				{
					Point point = new Point();
					point.basePosition = _base.position;
					point.tipPosition = _tip.position;
					point.timeCreated = Time.time;
					_points.Add(point);
					_lastPosition = ((Component)this).transform.position;
					if (_points.Count == 1)
					{
						_smoothedPoints.Add(point);
					}
					else if (_points.Count > 1)
					{
						for (int i = 0; i < 1 + subdivisions; i++)
						{
							_smoothedPoints.Add(point);
						}
					}
					if (_points.Count >= 4)
					{
						global::System.Collections.Generic.IEnumerable<Vector3> enumerable = Interpolate.NewCatmullRom((Vector3[])(object)new Vector3[4]
						{
							_points[_points.Count - 4].tipPosition,
							_points[_points.Count - 3].tipPosition,
							_points[_points.Count - 2].tipPosition,
							_points[_points.Count - 1].tipPosition
						}, subdivisions, false);
						global::System.Collections.Generic.IEnumerable<Vector3> enumerable2 = Interpolate.NewCatmullRom((Vector3[])(object)new Vector3[4]
						{
							_points[_points.Count - 4].basePosition,
							_points[_points.Count - 3].basePosition,
							_points[_points.Count - 2].basePosition,
							_points[_points.Count - 1].basePosition
						}, subdivisions, false);
						List<Vector3> val4 = new List<Vector3>(enumerable);
						List<Vector3> val5 = new List<Vector3>(enumerable2);
						float timeCreated = _points[_points.Count - 4].timeCreated;
						float timeCreated2 = _points[_points.Count - 1].timeCreated;
						for (int j = 0; j < val4.Count; j++)
						{
							int num = _smoothedPoints.Count - (val4.Count - j);
							if (num > -1 && num < _smoothedPoints.Count)
							{
								Point point2 = new Point();
								point2.basePosition = val5[j];
								point2.tipPosition = val4[j];
								point2.timeCreated = Mathf.Lerp(timeCreated, timeCreated2, (float)j / (float)val4.Count);
								_smoothedPoints[num] = point2;
							}
						}
					}
				}
				else
				{
					_points[_points.Count - 1].basePosition = _base.position;
					_points[_points.Count - 1].tipPosition = _tip.position;
					_smoothedPoints[_smoothedPoints.Count - 1].basePosition = _base.position;
					_smoothedPoints[_smoothedPoints.Count - 1].tipPosition = _tip.position;
				}
			}
			else
			{
				if (_points.Count > 0)
				{
					_points[_points.Count - 1].basePosition = _base.position;
					_points[_points.Count - 1].tipPosition = _tip.position;
				}
				if (_smoothedPoints.Count > 0)
				{
					_smoothedPoints[_smoothedPoints.Count - 1].basePosition = _base.position;
					_smoothedPoints[_smoothedPoints.Count - 1].tipPosition = _tip.position;
				}
			}
		}
		RemoveOldPoints(_points);
		if (_points.Count == 0)
		{
			_trailMesh.Clear();
		}
		RemoveOldPoints(_smoothedPoints);
		if (_smoothedPoints.Count == 0)
		{
			_trailMesh.Clear();
		}
		List<Point> smoothedPoints = _smoothedPoints;
		if (smoothedPoints.Count <= 1)
		{
			return;
		}
		Vector3[] array = (Vector3[])(object)new Vector3[smoothedPoints.Count * 2];
		Vector2[] array2 = (Vector2[])(object)new Vector2[smoothedPoints.Count * 2];
		int[] array3 = new int[(smoothedPoints.Count - 1) * 6];
		Color[] array4 = (Color[])(object)new Color[smoothedPoints.Count * 2];
		for (int k = 0; k < smoothedPoints.Count; k++)
		{
			Point point3 = smoothedPoints[k];
			float num2 = (Time.time - point3.timeCreated) / _lifeTime;
			Color val6 = Color.Lerp(Color.white, Color.clear, num2);
			if (_colors != null && _colors.Length > 0)
			{
				float num3 = num2 * (float)(_colors.Length - 1);
				float num4 = Mathf.Floor(num3);
				float num5 = Mathf.Clamp(Mathf.Ceil(num3), 1f, (float)(_colors.Length - 1));
				float num6 = Mathf.InverseLerp(num4, num5, num3);
				if (num4 >= (float)_colors.Length)
				{
					num4 = _colors.Length - 1;
				}
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				if (num5 >= (float)_colors.Length)
				{
					num5 = _colors.Length - 1;
				}
				if (num5 < 0f)
				{
					num5 = 0f;
				}
				val6 = Color.Lerp(_colors[(int)num4], _colors[(int)num5], num6);
			}
			float num7 = 0f;
			if (_sizes != null && _sizes.Length > 0)
			{
				float num8 = num2 * (float)(_sizes.Length - 1);
				float num9 = Mathf.Floor(num8);
				float num10 = Mathf.Clamp(Mathf.Ceil(num8), 1f, (float)(_sizes.Length - 1));
				float num11 = Mathf.InverseLerp(num9, num10, num8);
				if (num9 >= (float)_sizes.Length)
				{
					num9 = _sizes.Length - 1;
				}
				if (num9 < 0f)
				{
					num9 = 0f;
				}
				if (num10 >= (float)_sizes.Length)
				{
					num10 = _sizes.Length - 1;
				}
				if (num10 < 0f)
				{
					num10 = 0f;
				}
				num7 = Mathf.Lerp(_sizes[(int)num9], _sizes[(int)num10], num11);
			}
			Vector3 val7 = point3.tipPosition - point3.basePosition;
			array[k * 2] = point3.basePosition - val7 * (num7 * 0.5f);
			array[k * 2 + 1] = point3.tipPosition + val7 * (num7 * 0.5f);
			array4[k * 2] = (array4[k * 2 + 1] = val6);
			float num12 = (float)k / (float)smoothedPoints.Count;
			array2[k * 2] = new Vector2(num12, 0f);
			array2[k * 2 + 1] = new Vector2(num12, 1f);
			if (k > 0)
			{
				array3[(k - 1) * 6] = k * 2 - 2;
				array3[(k - 1) * 6 + 1] = k * 2 - 1;
				array3[(k - 1) * 6 + 2] = k * 2;
				array3[(k - 1) * 6 + 3] = k * 2 + 1;
				array3[(k - 1) * 6 + 4] = k * 2;
				array3[(k - 1) * 6 + 5] = k * 2 - 1;
			}
		}
		_trailMesh.Clear();
		_trailMesh.vertices = array;
		_trailMesh.colors = array4;
		_trailMesh.uv = array2;
		_trailMesh.triangles = array3;
	}

	private void RemoveOldPoints(List<Point> pointList)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		List<Point> val = new List<Point>();
		var enumerator = pointList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Point current = enumerator.Current;
				if (Time.time - current.timeCreated > _lifeTime)
				{
					val.Add(current);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator).Dispose();
		}
		var enumerator2 = val.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Point current2 = enumerator2.Current;
				pointList.Remove(current2);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2).Dispose();
		}
	}

	private void OnShieldStart()
	{
		_trailObject.GetComponent<Renderer>().material = _materialOnShield;
	}

	private void OnShieldFinish()
	{
		_trailObject.GetComponent<Renderer>().material = _material;
	}

	private void OnPutObjectIntoPool()
	{
		if ((Object)(object)_trailObject != (Object)null)
		{
			Object.Destroy((Object)(object)_trailObject);
			_trailObject = null;
		}
	}

	private void OnGetObjectFromPool()
	{
		if (!Utility.IsGoodPerformance())
		{
			Object.Destroy((Object)(object)this);
		}
		if ((Object)(object)_trailObject == (Object)null)
		{
			MakeTrail();
		}
	}
}
