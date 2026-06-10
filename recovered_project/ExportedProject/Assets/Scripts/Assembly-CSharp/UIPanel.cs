using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/UI/Panel")]
[ExecuteInEditMode]
public class UIPanel : MonoBehaviour
{
	public enum DebugInfo
	{
		None = 0,
		Gizmos = 1,
		Geometry = 2
	}

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool depthPass;

	public bool widgetsAreStatic;

	[HideInInspector]
	[SerializeField]
	private DebugInfo mDebugInfo = DebugInfo.Gizmos;

	[SerializeField]
	[HideInInspector]
	private UIDrawCall.Clipping mClipping;

	[SerializeField]
	[HideInInspector]
	private Vector4 mClipRange = Vector4.zero;

	[SerializeField]
	[HideInInspector]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	private OrderedDictionary mChildren = new OrderedDictionary();

	private BetterList<UIWidget> mWidgets = new BetterList<UIWidget>();

	private BetterList<Material> mChanged = new BetterList<Material>();

	private BetterList<UIDrawCall> mDrawCalls = new BetterList<UIDrawCall>();

	private BetterList<Vector3> mVerts = new BetterList<Vector3>();

	private BetterList<Vector3> mNorms = new BetterList<Vector3>();

	private BetterList<Vector4> mTans = new BetterList<Vector4>();

	private BetterList<Vector2> mUvs = new BetterList<Vector2>();

	private BetterList<Color32> mCols = new BetterList<Color32>();

	private Transform mTrans;

	private Camera mCam;

	private int mLayer = -1;

	private bool mDepthChanged;

	private bool mRebuildAll;

	private bool mChangedLastFrame;

	private bool mWidgetsAdded;

	private float mMatrixTime;

	private Matrix4x4 mWorldToLocal = Matrix4x4.identity;

	private static float[] mTemp = new float[4];

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private List<Transform> mRemoved = new List<Transform>();

	private bool mCheckVisibility;

	private float mCullTime;

	private bool mCulled;

	private static BetterList<UINode> mHierarchy = new BetterList<UINode>();

	public Transform cachedTransform
	{
		get
		{
			if ((Object)(object)mTrans == (Object)null)
			{
				mTrans = ((Component)this).transform;
			}
			return mTrans;
		}
	}

	public bool changedLastFrame
	{
		get
		{
			return mChangedLastFrame;
		}
	}

	public DebugInfo debugInfo
	{
		get
		{
			return mDebugInfo;
		}
		set
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (mDebugInfo != value)
			{
				mDebugInfo = value;
				BetterList<UIDrawCall> betterList = drawCalls;
				HideFlags hideFlags = (HideFlags)((mDebugInfo != DebugInfo.Geometry) ? 13 : 12);
				int i = 0;
				for (int size = betterList.size; i < size; i++)
				{
					UIDrawCall uIDrawCall = betterList[i];
					GameObject gameObject = ((Component)uIDrawCall).gameObject;
					NGUITools.SetActiveSelf(gameObject, false);
					((Object)gameObject).hideFlags = hideFlags;
					NGUITools.SetActiveSelf(gameObject, true);
				}
			}
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return mClipping;
		}
		set
		{
			if (mClipping != value)
			{
				mCheckVisibility = true;
				mClipping = value;
				UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mClipRange;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (mClipRange != value)
			{
				mCullTime = ((mCullTime != 0f) ? (Time.realtimeSinceStartup + 0.15f) : 0.001f);
				mCheckVisibility = true;
				mClipRange = value;
				UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mClipSoftness;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (mClipSoftness != value)
			{
				mClipSoftness = value;
				UpdateDrawcalls();
			}
		}
	}

	public BetterList<UIWidget> widgets
	{
		get
		{
			return mWidgets;
		}
	}

	public BetterList<UIDrawCall> drawCalls
	{
		get
		{
			int num = mDrawCalls.size;
			while (num > 0)
			{
				UIDrawCall uIDrawCall = mDrawCalls[--num];
				if ((Object)(object)uIDrawCall == (Object)null)
				{
					mDrawCalls.RemoveAt(num);
				}
			}
			return mDrawCalls;
		}
	}

	private UINode GetNode(Transform t)
	{
		UINode result = null;
		if ((Object)(object)t != (Object)null && mChildren.Contains((object)t))
		{
			result = (UINode)mChildren[(object)t];
		}
		return result;
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		UpdateTransformMatrix();
		a = mWorldToLocal.MultiplyPoint3x4(a);
		b = mWorldToLocal.MultiplyPoint3x4(b);
		c = mWorldToLocal.MultiplyPoint3x4(c);
		d = mWorldToLocal.MultiplyPoint3x4(d);
		mTemp[0] = a.x;
		mTemp[1] = b.x;
		mTemp[2] = c.x;
		mTemp[3] = d.x;
		float num = Mathf.Min(mTemp);
		float num2 = Mathf.Max(mTemp);
		mTemp[0] = a.y;
		mTemp[1] = b.y;
		mTemp[2] = c.y;
		mTemp[3] = d.y;
		float num3 = Mathf.Min(mTemp);
		float num4 = Mathf.Max(mTemp);
		if (num2 < mMin.x)
		{
			return false;
		}
		if (num4 < mMin.y)
		{
			return false;
		}
		if (num > mMax.x)
		{
			return false;
		}
		if (num3 > mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(Vector3 worldPos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		UpdateTransformMatrix();
		Vector3 val = mWorldToLocal.MultiplyPoint3x4(worldPos);
		if (val.x < mMin.x)
		{
			return false;
		}
		if (val.y < mMin.y)
		{
			return false;
		}
		if (val.x > mMax.x)
		{
			return false;
		}
		if (val.y > mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(UIWidget w)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)w).enabled || !NGUITools.GetActive(((Component)w).gameObject) || w.color.a < 0.001f)
		{
			return false;
		}
		if (mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector2 relativeSize = w.relativeSize;
		Vector2 val = Vector2.Scale(w.pivotOffset, relativeSize);
		Vector2 val2 = val;
		val.x += relativeSize.x;
		val.y -= relativeSize.y;
		Transform val3 = w.cachedTransform;
		Vector3 a = val3.TransformPoint((Vector2)(val));
		Vector3 b = val3.TransformPoint((Vector2)(new Vector2(val.x, val2.y)));
		Vector3 c = val3.TransformPoint((Vector2)(new Vector2(val2.x, val.y)));
		Vector3 d = val3.TransformPoint((Vector2)(val2));
		return IsVisible(a, b, c, d);
	}

	public void MarkMaterialAsChanged(Material mat, bool sort)
	{
		if ((Object)(object)mat != (Object)null)
		{
			if (sort)
			{
				mDepthChanged = true;
			}
			if (!mChanged.Contains(mat))
			{
				mChanged.Add(mat);
				mChangedLastFrame = true;
			}
		}
	}

	public bool WatchesTransform(Transform t)
	{
		return (Object)(object)t == (Object)(object)cachedTransform || mChildren.Contains((object)t);
	}

	private UINode AddTransform(Transform t)
	{
		UINode uINode = null;
		UINode uINode2 = null;
		while ((Object)(object)t != (Object)null && (Object)(object)t != (Object)(object)cachedTransform)
		{
			if (mChildren.Contains((object)t))
			{
				if (uINode2 == null)
				{
					uINode2 = (UINode)mChildren[(object)t];
				}
				break;
			}
			uINode = new UINode(t);
			if (uINode2 == null)
			{
				uINode2 = uINode;
			}
			mChildren.Add((object)t, (object)uINode);
			t = t.parent;
		}
		return uINode2;
	}

	private void RemoveTransform(Transform t)
	{
		if (!((Object)(object)t != (Object)null))
		{
			return;
		}
		while (mChildren.Contains((object)t))
		{
			mChildren.Remove((object)t);
			t = t.parent;
			if ((Object)(object)t == (Object)null || (Object)(object)t == (Object)(object)mTrans || t.childCount > 1)
			{
				break;
			}
		}
	}

	public void AddWidget(UIWidget w)
	{
		if (!((Object)(object)w != (Object)null))
		{
			return;
		}
		UINode uINode = AddTransform(w.cachedTransform);
		if (uINode != null)
		{
			uINode.widget = w;
			if (!mWidgets.Contains(w))
			{
				mWidgets.Add(w);
				if (!mChanged.Contains(w.material))
				{
					mChanged.Add(w.material);
					mChangedLastFrame = true;
				}
				mDepthChanged = true;
				mWidgetsAdded = true;
			}
		}
		else
		{
			Debug.LogError((object)("Unable to find an appropriate UIRoot for " + NGUITools.GetHierarchy(((Component)w).gameObject) + "\nPlease make sure that there is at least one game object above this widget!"), (Object)(object)((Component)w).gameObject);
		}
	}

	public void RemoveWidget(UIWidget w)
	{
		if (!((Object)(object)w != (Object)null))
		{
			return;
		}
		UINode node = GetNode(w.cachedTransform);
		if (node != null)
		{
			if (node.visibleFlag == 1 && !mChanged.Contains(w.material))
			{
				mChanged.Add(w.material);
				mChangedLastFrame = true;
			}
			RemoveTransform(w.cachedTransform);
		}
		mWidgets.Remove(w);
	}

	private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		int i = 0;
		for (int size = drawCalls.size; i < size; i++)
		{
			UIDrawCall uIDrawCall = drawCalls.buffer[i];
			if ((Object)(object)uIDrawCall.material == (Object)(object)mat)
			{
				return uIDrawCall;
			}
		}
		UIDrawCall uIDrawCall2 = null;
		if (createIfMissing)
		{
			GameObject val = new GameObject("_UIDrawCall [" + ((Object)mat).name + "]");
			Object.DontDestroyOnLoad((Object)(object)val);
			val.layer = ((Component)this).gameObject.layer;
			uIDrawCall2 = val.AddComponent<UIDrawCall>();
			uIDrawCall2.material = mat;
			mDrawCalls.Add(uIDrawCall2);
		}
		return uIDrawCall2;
	}

	private void Start()
	{
		mLayer = ((Component)this).gameObject.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
		mCam = ((!((Object)(object)uICamera != (Object)null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
	}

	private void OnEnable()
	{
		int i = 0;
		for (int size = mWidgets.size; i < size; i++)
		{
			AddWidget(mWidgets.buffer[i]);
		}
		mRebuildAll = true;
	}

	private void OnDisable()
	{
		int num = mDrawCalls.size;
		while (num > 0)
		{
			UIDrawCall uIDrawCall = mDrawCalls.buffer[--num];
			if ((Object)(object)uIDrawCall != (Object)null)
			{
				NGUITools.DestroyImmediate((Object)(object)((Component)uIDrawCall).gameObject);
			}
		}
		mDrawCalls.Clear();
		mChanged.Clear();
		mChildren.Clear();
	}

	private int GetChangeFlag(UINode start)
	{
		int num = start.changeFlag;
		if (num == -1)
		{
			Transform parent = start.trans.parent;
			while (true)
			{
				if ((Object)(object)parent != (Object)null && mChildren.Contains((object)parent))
				{
					UINode uINode = (UINode)mChildren[(object)parent];
					num = uINode.changeFlag;
					parent = parent.parent;
					if (num == -1)
					{
						mHierarchy.Add(uINode);
						continue;
					}
					break;
				}
				num = 0;
				break;
			}
			int i = 0;
			for (int size = mHierarchy.size; i < size; i++)
			{
				UINode uINode2 = mHierarchy.buffer[i];
				uINode2.changeFlag = num;
			}
			mHierarchy.Clear();
		}
		return num;
	}

	private void UpdateTransformMatrix()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup != 0f && mMatrixTime == realtimeSinceStartup)
		{
			return;
		}
		mMatrixTime = realtimeSinceStartup;
		mWorldToLocal = cachedTransform.worldToLocalMatrix;
		if (mClipping != UIDrawCall.Clipping.None)
		{
			Vector2 val = default(Vector2);
			val = new Vector2(mClipRange.z, mClipRange.w);
			if (val.x == 0f)
			{
				val.x = ((!((Object)(object)mCam == (Object)null)) ? mCam.pixelWidth : ((float)Screen.width));
			}
			if (val.y == 0f)
			{
				val.y = ((!((Object)(object)mCam == (Object)null)) ? mCam.pixelHeight : ((float)Screen.height));
			}
			val *= 0.5f;
			mMin.x = mClipRange.x - val.x;
			mMin.y = mClipRange.y - val.y;
			mMax.x = mClipRange.x + val.x;
			mMax.y = mClipRange.y + val.y;
		}
	}

	private void UpdateTransforms()
	{
		mChangedLastFrame = false;
		bool flag = false;
		bool flag2 = Time.realtimeSinceStartup > mCullTime;
		if (!widgetsAreStatic || mWidgetsAdded || flag2 != mCulled)
		{
			int i = 0;
			for (int count = mChildren.Count; i < count; i++)
			{
				UINode uINode = (UINode)mChildren[i];
				if ((Object)(object)uINode.trans == (Object)null)
				{
					mRemoved.Add(uINode.trans);
				}
				else if (uINode.HasChanged())
				{
					uINode.changeFlag = 1;
					flag = true;
				}
				else
				{
					uINode.changeFlag = -1;
				}
			}
			int j = 0;
			for (int count2 = mRemoved.Count; j < count2; j++)
			{
				mChildren.Remove((object)mRemoved[j]);
			}
			mRemoved.Clear();
		}
		if (!mCulled && flag2)
		{
			mCheckVisibility = true;
		}
		if (mCheckVisibility || flag || mRebuildAll)
		{
			int k = 0;
			for (int count3 = mChildren.Count; k < count3; k++)
			{
				UINode uINode2 = (UINode)mChildren[k];
				if (!((Object)(object)uINode2.widget != (Object)null))
				{
					continue;
				}
				int num = 1;
				if (flag2 || flag)
				{
					if (uINode2.changeFlag == -1)
					{
						uINode2.changeFlag = GetChangeFlag(uINode2);
					}
					if (flag2)
					{
						num = ((!mCheckVisibility && uINode2.changeFlag != 1) ? uINode2.visibleFlag : (IsVisible(uINode2.widget) ? 1 : 0));
					}
				}
				if (uINode2.visibleFlag != num)
				{
					uINode2.changeFlag = 1;
				}
				if (uINode2.changeFlag == 1 && (num == 1 || uINode2.visibleFlag != 0))
				{
					uINode2.visibleFlag = num;
					Material material = uINode2.widget.material;
					if (!mChanged.Contains(material))
					{
						mChanged.Add(material);
						mChangedLastFrame = true;
					}
				}
			}
		}
		mCulled = flag2;
		mCheckVisibility = false;
		mWidgetsAdded = false;
	}

	private void UpdateWidgets()
	{
		int i = 0;
		for (int count = mChildren.Count; i < count; i++)
		{
			UINode uINode = (UINode)mChildren[i];
			UIWidget widget = uINode.widget;
			if (uINode.visibleFlag == 1 && (Object)(object)widget != (Object)null && widget.UpdateGeometry(ref mWorldToLocal, uINode.changeFlag == 1, generateNormals) && !mChanged.Contains(widget.material))
			{
				mChanged.Add(widget.material);
				mChangedLastFrame = true;
			}
			uINode.changeFlag = 0;
		}
	}

	public void UpdateDrawcalls()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		Vector4 zero = Vector4.zero;
		if (mClipping != UIDrawCall.Clipping.None)
		{
			zero = new Vector4(mClipRange.x, mClipRange.y, mClipRange.z * 0.5f, mClipRange.w * 0.5f);
		}
		if (zero.z == 0f)
		{
			zero.z = (float)Screen.width * 0.5f;
		}
		if (zero.w == 0f)
		{
			zero.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform platform = Application.platform;
		if ((int)platform == 2 || (int)platform == 5 || (int)platform == 7)
		{
			zero.x -= 0.5f;
			zero.y += 0.5f;
		}
		Transform val = cachedTransform;
		int i = 0;
		for (int size = mDrawCalls.size; i < size; i++)
		{
			UIDrawCall uIDrawCall = mDrawCalls.buffer[i];
			uIDrawCall.clipping = mClipping;
			uIDrawCall.clipRange = zero;
			uIDrawCall.clipSoftness = mClipSoftness;
			uIDrawCall.depthPass = depthPass;
			Transform transform = ((Component)uIDrawCall).transform;
			transform.position = val.position;
			transform.rotation = val.rotation;
			transform.localScale = val.lossyScale;
		}
	}

	private void Fill(Material mat)
	{
		int num = mWidgets.size;
		while (num > 0)
		{
			if ((Object)(object)mWidgets[--num] == (Object)null)
			{
				mWidgets.RemoveAt(num);
			}
		}
		int i = 0;
		for (int size = mWidgets.size; i < size; i++)
		{
			UIWidget uIWidget = mWidgets.buffer[i];
			if (uIWidget.visibleFlag != 1 || !((Object)(object)uIWidget.material == (Object)(object)mat))
			{
				continue;
			}
			UINode node = GetNode(uIWidget.cachedTransform);
			if (node != null)
			{
				if (generateNormals)
				{
					uIWidget.WriteToBuffers(mVerts, mUvs, mCols, mNorms, mTans);
				}
				else
				{
					uIWidget.WriteToBuffers(mVerts, mUvs, mCols, null, null);
				}
			}
			else
			{
				Debug.LogError((object)("No transform found for " + NGUITools.GetHierarchy(((Component)uIWidget).gameObject)), (Object)(object)this);
			}
		}
		if (mVerts.size > 0)
		{
			UIDrawCall drawCall = GetDrawCall(mat, true);
			drawCall.depthPass = depthPass;
			drawCall.Set(mVerts, (!generateNormals) ? null : mNorms, (!generateNormals) ? null : mTans, mUvs, mCols);
		}
		else
		{
			UIDrawCall drawCall2 = GetDrawCall(mat, false);
			if ((Object)(object)drawCall2 != (Object)null)
			{
				mDrawCalls.Remove(drawCall2);
				NGUITools.DestroyImmediate((Object)(object)((Component)drawCall2).gameObject);
			}
		}
		mVerts.Clear();
		mNorms.Clear();
		mTans.Clear();
		mUvs.Clear();
		mCols.Clear();
	}

	private void LateUpdate()
	{
		UpdateTransformMatrix();
		UpdateTransforms();
		if (mLayer != ((Component)this).gameObject.layer)
		{
			mLayer = ((Component)this).gameObject.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
			mCam = ((!((Object)(object)uICamera != (Object)null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
			SetChildLayer(cachedTransform, mLayer);
			int i = 0;
			for (int size = drawCalls.size; i < size; i++)
			{
				((Component)mDrawCalls.buffer[i]).gameObject.layer = mLayer;
			}
		}
		UpdateWidgets();
		if (mDepthChanged)
		{
			mDepthChanged = false;
			mWidgets.Sort(UIWidget.CompareFunc);
		}
		int j = 0;
		for (int size2 = mChanged.size; j < size2; j++)
		{
			Fill(mChanged.buffer[j]);
		}
		UpdateDrawcalls();
		mChanged.Clear();
		mRebuildAll = false;
	}

	public void Refresh()
	{
		UIWidget[] componentsInChildren = ((Component)this).GetComponentsInChildren<UIWidget>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].Update();
		}
		LateUpdate();
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		Vector2 minRect = default(Vector2);
		minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = default(Vector2);
		maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = default(Vector2);
		minArea = new Vector2(clipRange.x - num, clipRange.y - num2);
		Vector2 maxArea = default(Vector2);
		maxArea = new Vector2(clipRange.x + num, clipRange.y + num2);
		if (clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += clipSoftness.x;
			minArea.y += clipSoftness.y;
			maxArea.x -= clipSoftness.x;
			maxArea.y -= clipSoftness.y;
		}
		return (Vector2)(NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea));
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = CalculateConstrainOffset((Vector2)(targetBounds.min), (Vector2)(targetBounds.max));
		if (val.magnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += val;
				targetBounds.center = targetBounds.center + val;
				SpringPosition component = ((Component)target).GetComponent<SpringPosition>();
				if ((Object)(object)component != (Object)null)
				{
					((Behaviour)component).enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(((Component)target).gameObject, target.localPosition + val, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(cachedTransform, target);
		return ConstrainTargetToBounds(target, ref targetBounds, immediate);
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if ((Object)(object)((Component)child).GetComponent<UIPanel>() == (Object)null)
			{
				((Component)child).gameObject.layer = layer;
				SetChildLayer(child, layer);
			}
		}
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform val = trans;
		UIPanel uIPanel = null;
		while ((Object)(object)uIPanel == (Object)null && (Object)(object)trans != (Object)null)
		{
			uIPanel = ((Component)trans).GetComponent<UIPanel>();
			if ((Object)(object)uIPanel != (Object)null || (Object)(object)trans.parent == (Object)null)
			{
				break;
			}
			trans = trans.parent;
		}
		if (createIfMissing && (Object)(object)uIPanel == (Object)null && (Object)(object)trans != (Object)(object)val)
		{
			uIPanel = ((Component)trans).gameObject.AddComponent<UIPanel>();
			SetChildLayer(uIPanel.cachedTransform, ((Component)uIPanel).gameObject.layer);
		}
		return uIPanel;
	}

	public static UIPanel Find(Transform trans)
	{
		return Find(trans, true);
	}
}
