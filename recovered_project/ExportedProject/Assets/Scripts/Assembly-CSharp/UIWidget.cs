using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft = 0,
		Top = 1,
		TopRight = 2,
		Left = 3,
		Center = 4,
		Right = 5,
		BottomLeft = 6,
		Bottom = 7,
		BottomRight = 8
	}

	[SerializeField]
	[HideInInspector]
	private Material mMat;

	[HideInInspector]
	[SerializeField]
	private Texture mTex;

	[HideInInspector]
	[SerializeField]
	private Color mColor = Color.white;

	[HideInInspector]
	[SerializeField]
	private Pivot mPivot = Pivot.Center;

	[SerializeField]
	[HideInInspector]
	private int mDepth;

	private Transform mTrans;

	private UIPanel mPanel;

	protected bool mChanged = true;

	protected bool mPlayMode = true;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private int mVisibleFlag = -1;

	private UIGeometry mGeom = new UIGeometry();

	public Color color
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mColor;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (!mColor.Equals((object)value))
			{
				mColor = value;
				mChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return mColor.a;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Color val = mColor;
			val.a = value;
			color = val;
		}
	}

	public Pivot pivot
	{
		get
		{
			return mPivot;
		}
		set
		{
			if (mPivot != value)
			{
				mPivot = value;
				mChanged = true;
			}
		}
	}

	public int depth
	{
		get
		{
			return mDepth;
		}
		set
		{
			if (mDepth != value)
			{
				mDepth = value;
				if ((Object)(object)mPanel != (Object)null)
				{
					mPanel.MarkMaterialAsChanged(material, true);
				}
			}
		}
	}

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

	public virtual Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if ((Object)(object)mMat != (Object)(object)value)
			{
				if ((Object)(object)mMat != (Object)null && (Object)(object)mPanel != (Object)null)
				{
					mPanel.RemoveWidget(this);
				}
				mPanel = null;
				mMat = value;
				mTex = null;
				if ((Object)(object)mMat != (Object)null)
				{
					CreatePanel();
				}
			}
		}
	}

	public virtual Texture mainTexture
	{
		get
		{
			Material val = material;
			if ((Object)(object)val != (Object)null)
			{
				if ((Object)(object)val.mainTexture != (Object)null)
				{
					mTex = val.mainTexture;
				}
				else if ((Object)(object)mTex != (Object)null)
				{
					if ((Object)(object)mPanel != (Object)null)
					{
						mPanel.RemoveWidget(this);
					}
					mPanel = null;
					mMat.mainTexture = mTex;
					if (((Behaviour)this).enabled)
					{
						CreatePanel();
					}
				}
			}
			return mTex;
		}
		set
		{
			if (!((Object)(object)mMat == (Object)null) && !((Object)(object)mMat.mainTexture != (Object)(object)value))
			{
				return;
			}
			if ((Object)(object)mPanel != (Object)null)
			{
				mPanel.RemoveWidget(this);
			}
			mPanel = null;
			mTex = value;
			if ((Object)(object)mMat != (Object)null)
			{
				mMat.mainTexture = value;
				if (((Behaviour)this).enabled)
				{
					CreatePanel();
				}
			}
		}
	}

	public UIPanel panel
	{
		get
		{
			if ((Object)(object)mPanel == (Object)null)
			{
				CreatePanel();
			}
			return mPanel;
		}
		set
		{
			mPanel = value;
		}
	}

	public int visibleFlag
	{
		get
		{
			return mVisibleFlag;
		}
		set
		{
			mVisibleFlag = value;
		}
	}

	public virtual Vector2 pivotOffset
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.zero;
			if (mPivot == Pivot.Top || mPivot == Pivot.Center || mPivot == Pivot.Bottom)
			{
				zero.x = -0.5f;
			}
			else if (mPivot == Pivot.TopRight || mPivot == Pivot.Right || mPivot == Pivot.BottomRight)
			{
				zero.x = -1f;
			}
			if (mPivot == Pivot.Left || mPivot == Pivot.Center || mPivot == Pivot.Right)
			{
				zero.y = 0.5f;
			}
			else if (mPivot == Pivot.BottomLeft || mPivot == Pivot.Bottom || mPivot == Pivot.BottomRight)
			{
				zero.y = 1f;
			}
			return zero;
		}
	}

	public virtual Vector2 relativeSize
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Vector2.one;
		}
	}

	public virtual bool keepMaterial
	{
		get
		{
			return false;
		}
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	public virtual void MarkAsChanged()
	{
		mChanged = true;
		if ((Object)(object)mPanel != (Object)null && ((Behaviour)this).enabled && NGUITools.GetActive(((Component)this).gameObject) && !Application.isPlaying && (Object)(object)material != (Object)null)
		{
			mPanel.AddWidget(this);
			CheckLayer();
		}
	}

	private void CreatePanel()
	{
		if ((Object)(object)mPanel == (Object)null && ((Behaviour)this).enabled && NGUITools.GetActive(((Component)this).gameObject) && (Object)(object)material != (Object)null)
		{
			mPanel = UIPanel.Find(cachedTransform);
			if ((Object)(object)mPanel != (Object)null)
			{
				CheckLayer();
				mPanel.AddWidget(this);
				mChanged = true;
			}
		}
	}

	public void CheckLayer()
	{
		if ((Object)(object)mPanel != (Object)null && ((Component)mPanel).gameObject.layer != ((Component)this).gameObject.layer)
		{
			Debug.LogWarning((object)"You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", (Object)(object)this);
			((Component)this).gameObject.layer = ((Component)mPanel).gameObject.layer;
		}
	}

	public void CheckParent()
	{
		if (!((Object)(object)mPanel != (Object)null))
		{
			return;
		}
		bool flag = true;
		Transform parent = cachedTransform.parent;
		while ((Object)(object)parent != (Object)null && !((Object)(object)parent == (Object)(object)mPanel.cachedTransform))
		{
			if (!mPanel.WatchesTransform(parent))
			{
				flag = false;
				break;
			}
			parent = parent.parent;
		}
		if (!flag)
		{
			if (!keepMaterial || Application.isPlaying)
			{
				material = null;
			}
			mPanel = null;
			CreatePanel();
		}
	}

	protected virtual void Awake()
	{
		mPlayMode = Application.isPlaying;
	}

	private void OnEnable()
	{
		mChanged = true;
		if (!keepMaterial)
		{
			mMat = null;
			mTex = null;
		}
		mPanel = null;
	}

	private void Start()
	{
		OnStart();
		CreatePanel();
	}

	public void Update()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		CheckLayer();
		if ((Object)(object)mPanel == (Object)null)
		{
			CreatePanel();
		}
		Vector3 localScale = cachedTransform.localScale;
		if (localScale.z != 1f)
		{
			localScale.z = 1f;
			mTrans.localScale = localScale;
		}
	}

	private void OnDisable()
	{
		if (!keepMaterial)
		{
			material = null;
		}
		else if ((Object)(object)mPanel != (Object)null)
		{
			mPanel.RemoveWidget(this);
		}
		mPanel = null;
	}

	private void OnDestroy()
	{
		if ((Object)(object)mPanel != (Object)null)
		{
			mPanel.RemoveWidget(this);
			mPanel = null;
		}
	}

	public bool UpdateGeometry(ref Matrix4x4 worldToPanel, bool parentMoved, bool generateNormals)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)material == (Object)null)
		{
			return false;
		}
		if (OnUpdate() || mChanged)
		{
			mChanged = false;
			mGeom.Clear();
			OnFill(mGeom.verts, mGeom.uvs, mGeom.cols);
			if (mGeom.hasVertices)
			{
				Vector3 val = (Vector2)(pivotOffset);
				Vector2 val2 = relativeSize;
				val.x *= val2.x;
				val.y *= val2.y;
				mGeom.ApplyOffset(val);
				mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
			}
			return true;
		}
		if (mGeom.hasVertices && parentMoved)
		{
			mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		mGeom.WriteToBuffers(v, u, c, n, t);
	}

	public virtual void MakePixelPerfect()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localScale = cachedTransform.localScale;
		int num = Mathf.RoundToInt(localScale.x);
		int num2 = Mathf.RoundToInt(localScale.y);
		localScale.x = num;
		localScale.y = num2;
		localScale.z = 1f;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		if (num % 2 == 1 && (pivot == Pivot.Top || pivot == Pivot.Center || pivot == Pivot.Bottom))
		{
			localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
		}
		else
		{
			localPosition.x = Mathf.Round(localPosition.x);
		}
		if (num2 % 2 == 1 && (pivot == Pivot.Left || pivot == Pivot.Center || pivot == Pivot.Right))
		{
			localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
		}
		else
		{
			localPosition.y = Mathf.Round(localPosition.y);
		}
		cachedTransform.localPosition = localPosition;
		cachedTransform.localScale = localScale;
	}

	protected virtual void OnStart()
	{
	}

	public virtual bool OnUpdate()
	{
		return false;
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}
}
