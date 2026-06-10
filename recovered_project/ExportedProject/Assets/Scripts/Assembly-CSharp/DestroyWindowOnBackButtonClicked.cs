using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class DestroyWindowOnBackButtonClicked : MonoBehaviour
{
	public TweenPosition tweenPosition;

	private bool isClosing;

	private void OnGetObjectFromPool()
	{
		isClosing = false;
		tweenPosition.Play(true);
	}

	private void OnBackButtonClicked()
	{
		if (isClosing)
		{
			return;
		}
		isClosing = true;
		tweenPosition.Play(false);
		((Component)((Component)this).transform.root).BroadcastMessage("OnCloseWindow", (SendMessageOptions)1);
		NGUIUtility.Destroy(((Component)this).gameObject);
	}
}
