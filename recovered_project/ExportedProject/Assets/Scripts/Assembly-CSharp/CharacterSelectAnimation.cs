using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterSelectAnimation : MonoBehaviour
{
	private void OnWindowMoved(bool isFocused)
	{
		if (isFocused)
		{
			GetComponent<Animation>().Play("idle_002");
		}
		else
		{
			GetComponent<Animation>().CrossFade("idle_001", 0.1f);
		}
	}

	public void SendAnimationEvent(string tagName)
	{
		((Component)this).SendMessage(tagName, (SendMessageOptions)1);
	}

	private void BackToIdleMotion()
	{
		GetComponent<Animation>().CrossFade("idle_001", 0.3f);
	}
}
