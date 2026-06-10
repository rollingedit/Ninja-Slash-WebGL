using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = new Vector3(2f, -2f);

	public float duration = 0.2f;

	private Vector3 mPos;

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
			TweenPosition component = ((Component)tweenTarget).GetComponent<TweenPosition>();
			if ((Object)(object)component != (Object)null)
			{
				component.position = mPos;
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
		mPos = tweenTarget.localPosition;
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
			TweenPosition.Begin(((Component)tweenTarget).gameObject, duration, isPressed ? (mPos + pressed) : ((!UICamera.IsHighlighted(((Component)this).gameObject)) ? mPos : (mPos + hover))).method = UITweener.Method.EaseInOut;
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
			TweenPosition.Begin(((Component)tweenTarget).gameObject, duration, (!isOver) ? mPos : (mPos + hover)).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
