using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ButtonSound : MonoBehaviour
{
	public AudioClip clip;

	private void OnClick()
	{
		MonoSingleton<SoundManager>.instance.PlaySound(clip);
	}
}
