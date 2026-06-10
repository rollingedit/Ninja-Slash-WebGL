using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick = 0,
		OnMouseOver = 1,
		OnMouseOut = 2,
		OnPress = 3,
		OnRelease = 4
	}

	public AudioClip audioClip;

	public Trigger trigger;

	public float volume = 1f;

	public float pitch = 1f;

	private void OnHover(bool isOver)
	{
		if (((Behaviour)this).enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (((Behaviour)this).enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnClick()
	{
		if (((Behaviour)this).enabled && trigger == Trigger.OnClick)
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}
}
