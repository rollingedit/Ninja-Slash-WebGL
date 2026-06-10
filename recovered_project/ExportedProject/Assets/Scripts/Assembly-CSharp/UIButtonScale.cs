using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

	private bool mInitDone;

	private bool mStarted;

	private bool mHighlighted;

	private void Start()
	{
		mStarted = true;
	}

	private void OnEnable()
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
			TweenScale component = ((Component)tweenTarget).GetComponent<TweenScale>();
			if ((Object)(object)component != (Object)null)
			{
				component.scale = mScale;
				((Behaviour)component).enabled = false;
			}
		}
	}

	private void Init()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		mInitDone = true;
		if ((Object)(object)tweenTarget == (Object)null)
		{
			tweenTarget = ((Component)this).transform;
		}
		mScale = tweenTarget.localScale;
	}

	private void OnPress(bool isPressed)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenScale.Begin(((Component)tweenTarget).gameObject, duration, isPressed ? Vector3.Scale(mScale, pressed) : ((!UICamera.IsHighlighted(((Component)this).gameObject)) ? mScale : Vector3.Scale(mScale, hover))).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenScale.Begin(((Component)tweenTarget).gameObject, duration, (!isOver) ? mScale : Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
