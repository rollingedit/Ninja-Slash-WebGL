using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterSwipeInput : MonoBehaviour
{
	public float minSwipeDistancePixels;

	private bool pressStarted;

	private Vector2 pressStartPos;

	private bool isEnabled;

	private float doubleClickTime;

	private void Start()
	{
	}

	private void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (!isEnabled)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			pressStarted = true;
			pressStartPos = (Vector2)(Input.mousePosition);
		}
		if (!pressStarted)
		{
			return;
		}
		if (Input.GetMouseButtonUp(0))
		{
			pressStarted = false;
			if (Time.time - doubleClickTime < 0.2f)
			{
				((Component)this).SendMessage("OnDoShield");
				doubleClickTime = -1f;
				return;
			}
			doubleClickTime = Time.time;
		}
		TestForSwipeGesture();
	}

	private void TestForSwipeGesture()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (Vector2)(Input.mousePosition);
		float num = Vector2.Distance(val, pressStartPos);
		if (num > minSwipeDistancePixels)
		{
			float num2 = val.y - pressStartPos.y;
			float num3 = val.x - pressStartPos.x;
			float num4 = 57.29578f * Mathf.Atan2(num3, num2);
			num4 = (360f + num4 - 45f) % 360f;
			if (num4 < 100f)
			{
				((Component)this).SendMessage("OnDoRightDash");
			}
			else if (num4 < 180f)
			{
				((Component)this).SendMessage("OnDoSlide");
			}
			else if (num4 < 280f)
			{
				((Component)this).SendMessage("OnDoLeftDash");
			}
			else
			{
				((Component)this).SendMessage("OnDoJump");
			}
			pressStarted = false;
		}
	}

	private void OnGameStart()
	{
		isEnabled = true;
	}

	private void OnRestart()
	{
		isEnabled = true;
	}

	private void OnDead()
	{
		isEnabled = false;
	}

	private void OnPutObjectIntoPool()
	{
		isEnabled = false;
	}

	private void OnPause()
	{
		isEnabled = false;
		pressStarted = false;
	}

	private void OnResume()
	{
		isEnabled = true;
		pressStarted = false;
	}
}
