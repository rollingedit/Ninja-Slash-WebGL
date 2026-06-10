using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
	public GameObject tweenTarget;

	public Color hover = new Color(0.6f, 1f, 0.2f, 1f);

	public Color pressed = Color.grey;

	public float duration = 0.2f;

	protected Color mColor;

	protected bool mInitDone;

	protected bool mStarted;

	protected bool mHighlighted;

	public Color defaultColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mColor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			mColor = value;
		}
	}

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		mStarted = true;
		OnEnable();
	}

	protected virtual void OnEnable()
	{
		if (mStarted && mHighlighted)
		{
			OnHover(UICamera.IsHighlighted(((Component)this).gameObject));
		}
	}

	private void OnDisable()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)tweenTarget != (Object)null)
		{
			TweenColor component = tweenTarget.GetComponent<TweenColor>();
			if ((Object)(object)component != (Object)null)
			{
				component.color = mColor;
				((Behaviour)component).enabled = false;
			}
		}
	}

	protected void Init()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		mInitDone = true;
		if ((Object)(object)tweenTarget == (Object)null)
		{
			tweenTarget = ((Component)this).gameObject;
		}
		UIWidget component = tweenTarget.GetComponent<UIWidget>();
		if ((Object)(object)component != (Object)null)
		{
			mColor = component.color;
			return;
		}
		Renderer renderer = tweenTarget.GetComponent<Renderer>();
		if ((Object)(object)renderer != (Object)null)
		{
			mColor = renderer.material.color;
			return;
		}
		Light light = tweenTarget.GetComponent<Light>();
		if ((Object)(object)light != (Object)null)
		{
			mColor = light.color;
			return;
		}
		Debug.LogWarning((object)(NGUITools.GetHierarchy(((Component)this).gameObject) + " has nothing for UIButtonColor to color"), (Object)(object)this);
		((Behaviour)this).enabled = false;
	}

	protected virtual void OnPress(bool isPressed)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			TweenColor.Begin(tweenTarget, duration, isPressed ? pressed : ((!UICamera.IsHighlighted(((Component)this).gameObject)) ? mColor : hover));
		}
	}

	protected virtual void OnHover(bool isOver)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			TweenColor.Begin(tweenTarget, duration, (!isOver) ? mColor : hover);
			mHighlighted = isOver;
		}
	}
}
