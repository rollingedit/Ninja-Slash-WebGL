using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Root")]
public class UIRoot : MonoBehaviour
{
	private static List<UIRoot> mRoots = new List<UIRoot>();

	private Transform mTrans;

	public bool automatic = true;

	public int manualHeight = 800;

	public int minimumHeight = 320;

	public int maximumHeight = 1080;

	public static List<UIRoot> list
	{
		get
		{
			return mRoots;
		}
	}

	public int activeHeight
	{
		get
		{
			int num = Mathf.Max(2, Screen.height);
			if (automatic)
			{
				if (num < minimumHeight)
				{
					return minimumHeight;
				}
				if (num > maximumHeight)
				{
					return maximumHeight;
				}
				return num;
			}
			return manualHeight;
		}
	}

	public float pixelSizeAdjustment
	{
		get
		{
			float num = Screen.height;
			if (automatic)
			{
				if (num < (float)minimumHeight)
				{
					return (float)minimumHeight / num;
				}
				if (num > (float)maximumHeight)
				{
					return (float)maximumHeight / num;
				}
				return 1f;
			}
			return (float)manualHeight / num;
		}
	}

	private void Awake()
	{
		mTrans = ((Component)this).transform;
		mRoots.Add(this);
	}

	private void OnDestroy()
	{
		mRoots.Remove(this);
	}

	private void Start()
	{
		UIOrthoCamera componentInChildren = ((Component)this).GetComponentInChildren<UIOrthoCamera>();
		if ((Object)(object)componentInChildren != (Object)null)
		{
			Debug.LogWarning((object)"UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", (Object)(object)componentInChildren);
			Camera component = ((Component)componentInChildren).gameObject.GetComponent<Camera>();
			((Behaviour)componentInChildren).enabled = false;
			if ((Object)(object)component != (Object)null)
			{
				component.orthographicSize = 1f;
			}
		}
	}

	private void Update()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)mTrans != (Object)null)
		{
			float num = 2f / (float)activeHeight;
			Vector3 localScale = mTrans.localScale;
			if (!(Mathf.Abs(localScale.x - num) <= 1E-45f) || !(Mathf.Abs(localScale.y - num) <= 1E-45f) || !(Mathf.Abs(localScale.z - num) <= 1E-45f))
			{
				mTrans.localScale = new Vector3(num, num, num);
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		int i = 0;
		for (int count = mRoots.Count; i < count; i++)
		{
			UIRoot uIRoot = mRoots[i];
			if ((Object)(object)uIRoot != (Object)null)
			{
				((Component)uIRoot).BroadcastMessage(funcName, (SendMessageOptions)1);
			}
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError((object)"SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
			return;
		}
		int i = 0;
		for (int count = mRoots.Count; i < count; i++)
		{
			UIRoot uIRoot = mRoots[i];
			if ((Object)(object)uIRoot != (Object)null)
			{
				((Component)uIRoot).BroadcastMessage(funcName, param, (SendMessageOptions)1);
			}
		}
	}
}
