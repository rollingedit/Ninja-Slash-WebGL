using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	private void OnEnable()
	{
		if ((Object)(object)target != (Object)null)
		{
			target.spriteName = ((!UICamera.IsHighlighted(((Component)this).gameObject)) ? normalSprite : hoverSprite);
		}
	}

	private void Start()
	{
		if ((Object)(object)target == (Object)null)
		{
			target = ((Component)this).GetComponentInChildren<UISprite>();
		}
	}

	private void OnHover(bool isOver)
	{
		if (((Behaviour)this).enabled && (Object)(object)target != (Object)null)
		{
			target.spriteName = ((!isOver) ? normalSprite : hoverSprite);
			target.MakePixelPerfect();
		}
	}

	private void OnPress(bool pressed)
	{
		if (((Behaviour)this).enabled && (Object)(object)target != (Object)null)
		{
			target.spriteName = ((!pressed) ? normalSprite : pressedSprite);
			target.MakePixelPerfect();
		}
	}
}
