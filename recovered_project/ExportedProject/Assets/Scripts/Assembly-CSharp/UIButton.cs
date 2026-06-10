using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public Color disabledColor = Color.grey;

	public bool isEnabled
	{
		get
		{
			Collider collider = GetComponent<Collider>();
			return ((Object)(object)collider != (Object)null) && collider.enabled;
		}
		set
		{
			Collider collider = GetComponent<Collider>();
			if (((Object)(object)collider != (Object)null) && collider.enabled != value)
			{
				collider.enabled = value;
				UpdateColor(value, false);
			}
		}
	}

	protected override void OnEnable()
	{
		if (isEnabled)
		{
			base.OnEnable();
		}
		else
		{
			UpdateColor(false, true);
		}
	}

	protected override void OnHover(bool isOver)
	{
		if (isEnabled)
		{
			base.OnHover(isOver);
		}
	}

	protected override void OnPress(bool isPressed)
	{
		if (isEnabled)
		{
			base.OnPress(isPressed);
		}
	}

	public void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (((Object)(object)tweenTarget != (Object)null))
		{
			Color color = ((!shouldBeEnabled) ? disabledColor : base.defaultColor);
			TweenColor tweenColor = TweenColor.Begin(tweenTarget, 0.15f, color);
			if (immediate)
			{
				tweenColor.color = color;
				((Behaviour)tweenColor).enabled = false;
			}
		}
	}
}
