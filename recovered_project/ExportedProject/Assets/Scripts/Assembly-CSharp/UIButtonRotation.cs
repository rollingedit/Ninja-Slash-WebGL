using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = Vector3.zero;

	public float duration = 0.2f;

	private Quaternion mRot;

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
			TweenRotation component = ((Component)tweenTarget).GetComponent<TweenRotation>();
			if ((Object)(object)component != (Object)null)
			{
				component.rotation = mRot;
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
		mRot = tweenTarget.localRotation;
	}

	private void OnPress(bool isPressed)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenRotation.Begin(((Component)tweenTarget).gameObject, duration, isPressed ? (mRot * Quaternion.Euler(pressed)) : ((!UICamera.IsHighlighted(((Component)this).gameObject)) ? mRot : (mRot * Quaternion.Euler(hover)))).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenRotation.Begin(((Component)tweenTarget).gameObject, duration, (!isOver) ? mRot : (mRot * Quaternion.Euler(hover))).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
