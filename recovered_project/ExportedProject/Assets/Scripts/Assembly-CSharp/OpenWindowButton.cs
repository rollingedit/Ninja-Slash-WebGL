using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class OpenWindowButton : MonoBehaviour
{
	private static int s_openWindowCount;

	public static bool IsWindowOpen
	{
		get
		{
			return s_openWindowCount > 0;
		}
	}

	public static bool TryAcquireWindow()
	{
		if (IsWindowOpen)
		{
			return false;
		}
		s_openWindowCount = 1;
		return true;
	}

	public static void ReleaseWindow()
	{
		s_openWindowCount = Mathf.Max(0, s_openWindowCount - 1);
	}

	public GameObject targetWindowPrefab;

	public UIButton button;

	private void OnClick()
	{
		if ((Object)(object)targetWindowPrefab == (Object)null || !TryAcquireWindow())
		{
			return;
		}
		GameObject gameObject = ((Component)NGUITools.FindCameraForLayer(((Component)this).gameObject.layer)).gameObject;
		NGUIUtility.AddChild(gameObject, targetWindowPrefab);
		((Component)((Component)this).transform.root).BroadcastMessage("OnOpenWindow", (SendMessageOptions)1);
	}

	private void OnOpenWindow()
	{
		button.isEnabled = false;
	}

	private void OnCloseWindow()
	{
		ReleaseWindow();
		button.isEnabled = true;
	}
}
