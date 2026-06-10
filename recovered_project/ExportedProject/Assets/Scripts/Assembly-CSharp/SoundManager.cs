using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SoundManager : MonoSingleton<SoundManager>
{
	public AudioSource bgm;

	public AudioSource effectSoundObject;

	private void Start()
	{
		bgm.mute = !MonoSingleton<UserData>.instance.IsBGMOn;
		effectSoundObject.mute = !MonoSingleton<UserData>.instance.IsEffectSoundOn;
	}

	public void PlaySound(AudioClip clipIn)
	{
		Utility.PlaySound(((Component)effectSoundObject).GetComponent<AudioSource>(), clipIn);
	}

	public void OnChangeBGMState()
	{
		bgm.mute = !MonoSingleton<UserData>.instance.IsBGMOn;
	}

	public void OnChangeEffectSoundState()
	{
		effectSoundObject.mute = !MonoSingleton<UserData>.instance.IsEffectSoundOn;
	}
}
